using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class MovementNode {

	//[XmlAttribute("direction")]
	public Vector2 direction;

	//[XmlAttribute("force")]
	public float force;

	//[XmlAttribute("delay")]
	public float delay;

	//[XmlAttribute("sesgment")]
	public int segment;

	public MovementNode(Vector2 dir, float f, float del, int s)
	{
		direction = dir;
		force = f;
		delay = del;
		segment = s;
	}

	public Vector2 Direction 
	{
		get
		{
			return direction;
		}
		set
		{
			direction = value;
		}
	}

	public float Force
	{
		get
		{
			return force;
		}
		set
		{
			force = value;
		}
	}

	public float Delay
	{
		get
		{
			return delay;
		}
		set
		{
			delay = value;
		}
	}

	public int Segment
	{
		get
		{
			return segment;
		}
		set
		{
			segment = value;
		}
	}
}
