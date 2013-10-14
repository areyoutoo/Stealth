using UnityEngine;
using System.Collections;

public struct IntVector2 {
	int x;
	int y;
	
	public float magnitude {
		get {
			return Mathf.Sqrt(x*x + y*y);
		}
	}
	
	public int gridMagnitude {
		get {
			return Mathf.Abs(x) + Mathf.Abs(y);
		}
	}
	
	public IntVector2(int x, int y) {
		this.x = x;
		this.y = y;
	}
	
	public static IntVector2 north {
		get {
			return new IntVector2(0,1);
		}
	}
	
	public static IntVector2 south {
		get {
			return new IntVector2(0,-1);
		}
	}
	
	public static IntVector2 east {
		get {
			return new IntVector2(1,0);
		}
	}
	
	public static IntVector2 west {
		get {
			return new IntVector2(-1,0);
		}
	}
	
	public static IntVector2 operator +(IntVector2 a, IntVector2 b) {
		return new IntVector2(a.x + b.x, a.y + b.y);
	}
	
	public static IntVector2 operator -(IntVector2 a, IntVector2 b) {
		return new IntVector2(a.x - b.x, a.y - b.y);
	}
	
	public static IntVector2 operator -(IntVector2 a) {
		return new IntVector2(-a.x, -a.y);
	}
	
	public static IntVector2 operator *(IntVector2 a, IntVector2 b) {
		return new IntVector2(a.x * b.x, a.y * b.y);
	}
	
	public static IntVector2 operator *(IntVector2 a, int scale) {
		return new IntVector2(a.x * scale, a.y * scale);
	}
}
