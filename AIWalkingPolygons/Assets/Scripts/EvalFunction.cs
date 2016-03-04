using UnityEngine;
using System.Collections;

public class EvalFunction
{

	public double evalFunction (Creature c) {
		double distance = 0.0;
		distance = c.getStart() - c.getEnd();
		return distance;
	}
}
