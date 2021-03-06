﻿using UnityEngine;
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
	
	public Vector2 ToVector2() {
		return new UnityEngine.Vector2(x * 1f, y * 1f);
	}
	
	public Vector3 ToVector3(float z) {
		return new UnityEngine.Vector3(x * 1f, y * 1f, z);
	}
	
	public override string ToString() {
		return string.Format("({0},{1})", x, y);
	}
	
	public override bool Equals(object other) {
		if (other is IntVector2) {
			return this == (IntVector2)other;
		} else {
			return base.Equals(other);
		}
	}
	
	public override int GetHashCode() {
		return base.GetHashCode();
	}
	
	public bool Equals(IntVector2 other) {
		return this == other;
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
	
	public static IntVector2 zero {
		get {
			return new IntVector2(0,0);
		}
	}
	
	public static IntVector2 one {
		get {
			return new IntVector2(1,1);
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
	
	public static IntVector2 operator *(IntVector2 a, float scale) {
		return a * Mathf.FloorToInt(scale);
	}
	
	public static bool operator ==(IntVector2 a, IntVector2 b) {
		if ((object)a == null) {
			return (object)b == null;
		} else if ((object)b == null) {
			return false;
		} else {
			return a.x == b.x && a.y == b.y;
		}
	}
	
	public static bool operator !=(IntVector2 a, IntVector2 b) {
		return !(a==b);
	}
}
