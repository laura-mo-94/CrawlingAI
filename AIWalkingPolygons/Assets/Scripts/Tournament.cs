using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tournament : MonoBehaviour {

	public int populationSize;
	public float generationTime;
	public int numPassedOn;
	public float minForce;
	public float maxForce;
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
}
