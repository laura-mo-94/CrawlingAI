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

	Vector3 start;
	Vector3 end;

	public void Awake()
	{
		current = 0;
		currentDelay = 0f;
		CreateCreature ();
	}

	public void CreateCreature()
	{
		segments = new List<GameObject> ();
		joints = new List<HingeJoint2D> ();
		rigidBodies = new List<Rigidbody2D> ();
		segmentMeshes = new List<PolygonMesh> ();

		foreach(Transform child in transform)
		{
			segments.Add (child.gameObject);
			PolygonMesh segmentMesh = child.GetComponent<PolygonMesh>();
			segmentMesh.CreateMesh();
			segmentMeshes.Add(segmentMesh);
			rigidBodies.Add(child.GetComponent<Rigidbody2D>());
		}

		start = segments[1].transform.position;
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

			if(current >= sequence.Count)
			{
				current = 0;
			}
		}
		else
		{
			currentDelay += Time.deltaTime;
		}
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

	public Vector3 Start
	{
		get 
		{
			return start;
		}
		set
		{
			start = value;
		}
	}

	public Vector3 End
	{
		get
		{
			return end;
		}
		set
		{
			end = value;
		}
	}

	public Vector3 getPosition()
	{
		return segments [1].transform.localPosition;
	}

}
