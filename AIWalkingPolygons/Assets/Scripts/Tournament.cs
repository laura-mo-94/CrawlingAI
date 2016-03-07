using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tournament : MonoBehaviour {

    public static Tournament instance;
	public int populationSize;
	public float generationTime;
	public int numPassedOn;
	public float minForce;
	public float maxForce;
	public int sequenceLength;
	public float minDelay;
	public float maxDelay;

	public GameObject creaturePrefab;
	public GameObject environmentPrefab;

	public int maxGenerations;
	public List<List<GameObject>> environmentPieces;
	public Text generationLabel;

	int rowSize;
	int currentGeneration;

	List<Creature> generation;

	float currentTime;
	GeneticEvolution evolution;
	Camera mainCam;

	// Use this for initialization
	void Start () {
        instance = this;
		currentTime = 0;
		mainCam = Camera.main;
		currentGeneration = 0;
		rowSize = Mathf.CeilToInt (Mathf.Sqrt (populationSize));

		environmentPieces = new List<List<GameObject>> ();

		generation = CreateCreatures ();
		evolution = new GeneticEvolution ();
		generationLabel.text = "Generation: " + currentGeneration;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentGeneration < maxGenerations) 
		{
			if (currentTime < generationTime) 
			{
				Vector3 pos = generation[0].getPosition();
				mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(pos.x, pos.y, mainCam.transform.position.z), 5f * Time.deltaTime);
				runSequences ();
			} 
			else 
			{
                for(int i = 0; i < generation.Count; i++)
                {
                    generation[i].End = generation[i].getPosition();
                }
				currentGeneration ++;
				currentTime = 0f;
				generationLabel.text = "Generation: " + currentGeneration;
				List<Creature> nextGen = evolution.CreateNextGen(generation);

				replaceCreatures(nextGen);
			}

		}
	}

	public List<Creature> CreateCreatures()
	{
		int column = 0;
		int row = 0;

		List<Creature> creatures = new List<Creature> ();

		for(int i = 0; i < populationSize; i++)
		{
			if(column == 0)
			{
				environmentPieces.Add(new List<GameObject>());
			}

			GameObject env = GameObject.Instantiate(environmentPrefab);
			env.transform.position = new Vector2(row * 40f, column * 40f);
			environmentPieces[row].Add(env);
			env.transform.parent = transform;

			GameObject c = GameObject.Instantiate(creaturePrefab);
			Creature creature = c.GetComponent<Creature>();
			creature.display(false);
			creature.Sequence = createSequence(creature);
			c.transform.parent = env.transform;
			creature.transform.localPosition = Vector3.zero;
			creatures.Add (creature);

			column++;
			if(column >= rowSize)
            {
				column = 0;
				row ++;
			}
		}
        
		creatures [0].display (true);
		return creatures;
	}

	public List<MovementNode> createSequence(Creature creature)
	{
		List<MovementNode> sequence = new List<MovementNode>();

		for(int i = 0; i < sequenceLength; i++)
		{
			Vector2 dir = new Vector2(Random.Range(0f, 0.5f), Random.Range (0.5f, 2f));
			dir.Normalize();
			float force = Random.Range (minForce, maxForce);
			int segment = Random.Range(0, creature.getSegmentCount());
			float delay = Random.Range(minDelay, maxDelay);
			sequence.Add(new MovementNode(dir, force, delay, segment));
		}

		return sequence;
	}

	public void runSequences()
	{
		for(int i = 0; i < generation.Count; i++)
		{
			generation[i].runSequence ();
		}

		currentTime += Time.deltaTime;
	}

	public void replaceCreatures(List<Creature> nextGen)
	{
		List<List<MovementNode>> sequences = new List<List<MovementNode>> ();

		for(int x = 0; x < generation.Count; x++)
		{
			sequences.Add (nextGen[x].Sequence);
            GameObject.Destroy(generation[x].gameObject);
		}

		generation.Clear ();

		int rowSize = Mathf.CeilToInt (Mathf.Sqrt (sequences.Count));

		int current = 0;


		for (int i = 0; i < rowSize; i++)
		{
			for(int j = 0; j < rowSize; j++)
			{
				if(generation.Count < sequences.Count)
				{
					GameObject c = GameObject.Instantiate(creaturePrefab);
					Creature creature = c.GetComponent<Creature>();
					creature.display(false);
					creature.Sequence = sequences[current];
					current ++;
					c.transform.parent = environmentPieces[i][j].transform;
					creature.transform.localPosition = Vector3.zero;
                    creature.Start = creature.getPosition();
                    generation.Add (creature);

				}
			}
		}
 
		generation [0].display (true);
	}
}
