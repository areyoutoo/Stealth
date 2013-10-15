using UnityEngine;
using System.Collections;

public static class Vector3Extensions {
	public static Vector3 WithX(this Vector3 v, float x) {
		return new Vector3(x, v.y, v.z);
	}
	
	public static Vector3 WithY(this Vector3 v, float y) {
		return new Vector3(v.z, y, v.z);
	}
	
	public static Vector3 WithZ(this Vector3 v, float z) {
		return new Vector3(v.x, v.y, z);
	}
	
	public static Vector2 ToVector2(this Vector3 v) {
		return new Vector2(v.x, v.y);
	}
}
