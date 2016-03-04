using UnityEngine;
using System.Collections;

public class EvalFunction : MonoBehaviour {

	int evalFunction (Creature c) {
		int distance = 0;
		distance = c.getStart() - c.getEnd();
		return distance;
	}
}
