using UnityEngine;
using System.Collections;

public static class Randomx {
	public static Vector3 InBounds(Bounds b) {
		Vector3 v = Vector3.zero;
		for (int i=0; i<3; i++) {
			v[i] = Random.Range(b.min[i], b.max[i]);
		}
		Debug.Log(v);
		return v;
	}
	
	public static Vector3 OnLine(Vector3 from, Vector3 to) {
		return Vector3.Lerp(from, to, Random.value);
	}
}
