using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour 
{
	List<GameObject> segments;
	List<HingeJoint2D> joints;

	public float force;
	// Use this for initialization
	void Start () 
	{
		segments = new List<GameObject> ();
		joints = new List<HingeJoint2D> ();
		foreach(Transform child in transform)
		{
			segments.Add (child.gameObject);
		}

		LinkSegments (segments);
	}
	
	// Update is called once per frame
	void Update () {
		checkMove ();
	}

	void LinkSegments(List<GameObject> segments)
	{
		for(int i = 0; i < segments.Count - 1; i++)
		{
			HingeJoint2D joint = segments[i].AddComponent<HingeJoint2D>();
			PolygonMesh mesh1 = segments[i].GetComponent<PolygonMesh>();
			joint.anchor = mesh1.getRightMostVertice();

			joint.connectedBody = segments[i+1].GetComponent<Rigidbody2D>();
			PolygonMesh mesh2 = segments[i+1].GetComponent<PolygonMesh>();
			joint.connectedAnchor = mesh2.getLeftMostVertice();

			joint.enableCollision = true;
			joints.Add(joint);
		}
	}

	void checkMove()
	{
		if(Input.GetKeyDown(KeyCode.Alpha0))
		{
			segments[0].GetComponent<Rigidbody2D>().AddForce(force * Vector2.up, ForceMode2D.Impulse);
		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			segments[1].GetComponent<Rigidbody2D>().AddForce(force * Vector2.up, ForceMode2D.Impulse);
		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			segments[2].GetComponent<Rigidbody2D>().AddForce(force * Vector2.up, ForceMode2D.Impulse);
		}
	}
}
