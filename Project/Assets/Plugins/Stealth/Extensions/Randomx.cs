using UnityEngine;
using System.Collections;

public static class Randomx {
	public static Vector3 InBounds(Bounds b) {
		Vector3 v = Vector3.zero;
		for (int i=0; i<3; i++) {
			v[i] = Random.Range(b.min[i], b.max[i]);
		}
		return v;
	}
	
	public static Vector3 OnLine(Vector3 from, Vector3 to) {
		return Vector3.Lerp(from, to, Random.value);
	}
	
	public static float AbsRange(float min, float max) {
		float f = Random.Range(min, max);
		return Random.value < 0.5f ? f : -f;
	}
	
	public static int AbsRange(int min, int max) {
		int i = Random.Range(min, max);
		return Random.value < 0.5f ? i : -i;
	}
	
	public static bool CoinToss() {
		return Random.value < 0.5f;
	}
}
