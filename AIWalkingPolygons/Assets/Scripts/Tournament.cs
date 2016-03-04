using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tournament : MonoBehaviour {

	public int populationSize;
	public float generationTime;
	public int numPassedOn;
	public float minForce;
	public float maxForce;
	public int sequenceLength;

	public GameObject creaturePrefab;

	List<Creature> generation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<Creature> CreateCreatures()
	{
		List<Creature> creatures = new List<Creature> ();

		for(int i = 0; i < populationSize; i++)
		{
			GameObject c = GameObject.Instantiate(creaturePrefab);
			Creature creature = c.GetComponent<Creature>();
			creature.display(false);
			creatures.Add (creature);
		}

		creatures [0].display (true);
		return creatures;
	}

	public List<MovementNode> createSequence(Creature creature)
	{
		List<MovementNode> sequence = new List<MovementNode>();

		for(int i = 0; i < sequenceLength; i++)
		{
			Vector2 dir = new Vector2(Random.Range(0f, 2f), Random.Range (0f, 2f));
			dir.Normalize();
			float force = Random.Range (minForce, maxForce);
			float segment = Random.Range(0, creature.getSegmentCount());
			float delay = Random.Range(0f, 1f);

		}

		return sequence;
	}
}
