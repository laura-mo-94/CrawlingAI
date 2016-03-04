using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour 
{
	List<GameObject> segments;
	List<PolygonMesh> segmentMeshes;
	List<Rigidbody2D> rigidBodies;
	List<HingeJoint2D> joints;

	public float force;
	public float forceLimit;

	float totalForce;
	List<MovementNode> sequence;

	int current;
	float currentDelay;

	public Creature(List<MovementNode> seq)
	{
		sequence = seq;
		current = 0;
		currentDelay = 0f;
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		runSequence ();
	}

	public void CreateCreature(bool display)
	{
		segments = new List<GameObject> ();
		joints = new List<HingeJoint2D> ();
		rigidBodies = new List<Rigidbody2D> ();
		
		foreach(Transform child in transform)
		{
			segments.Add (child.gameObject);
			segmentMeshes.Add(child.GetComponent<PolygonMesh>());
			rigidBodies.Add(child.GetComponent<Rigidbody2D>());
		}
		
		LinkSegments (segments);
	}

	void LinkSegments(List<GameObject> segments)
	{
		for(int i = 0; i < segments.Count - 1; i++)
		{
			HingeJoint2D joint = segments[i].AddComponent<HingeJoint2D>();
			PolygonMesh mesh1 = segmentMeshes[i];
			joint.anchor = mesh1.getRightMostVertice();

			joint.connectedBody = segments[i+1].GetComponent<Rigidbody2D>();
			PolygonMesh mesh2 = segmentMeshes[i+1];
			joint.connectedAnchor = mesh2.getLeftMostVertice();

			joint.enableCollision = true;
			joints.Add(joint);
		}
	}

	public void runSequence()
	{
		if(currentDelay >= sequence[current].Delay)
		{
			rigidBodies[sequence[current].Segment].AddForce(sequence[current].Force * sequence[current].Direction, ForceMode2D.Impulse);
			currentDelay = 0f;
			current ++;
		}
		else
		{
			currentDelay += Time.deltaTime;
		}
	}

	public float getTotalForce()
	{
		totalForce = 0;

		for(int i = 0; i < segments.Count; i++)
		{
			totalForce  = totalForce + (rigidBodies[i].velocity.magnitude * rigidBodies[i].transform.up.y);
		}

		return totalForce;
	}

	public void display(bool d)
	{
		for(int i = 0; i < segmentMeshes.Count; i ++)
		{
			segmentMeshes[i].displayMesh(d);
		}
	}

	public int getSegmentCount()
	{
		return segments.Count;
	}

	public List<MovementNode> Sequence
	{
		get{
			return sequence;
		}
		set{
			sequence = value;
		}
	}
}
