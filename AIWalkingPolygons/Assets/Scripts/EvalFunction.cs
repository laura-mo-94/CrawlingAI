using UnityEngine;
using System.Collections;

public class EvalFunction
{

	public double evalFunction (Creature c) {
		double distance = 0.0;
		distance = c.End.x - c.Start.x;
		return distance;
	}
}
