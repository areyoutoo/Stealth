using UnityEngine;
using System.Collections.Generic;

public enum RoomType {
	Room,
	Hallway,
	Stub,
}

public class RoomInfo : MonoBehaviour {
	public Bounds bounds { get; protected set; }
	public Bounds innerBounds { get; protected set; }
	public RoomType roomType { get; protected set; }
	public IntVector2 coord { get; protected set; }
	
	public void Init(IntVector2 coord) {
		this.coord = coord;
	}
	
	protected void Start() {
		Bounds b = new Bounds(transform.position, Vector3.zero);
		foreach (Collider c in GetComponentsInChildren<Collider>()) {
			b.Encapsulate(c.bounds);
		}
		bounds = b;
		innerBounds = new Bounds(transform.position, b.size - Vector3.one - Vector3.one);
	}
	
	void SpawnGold() {
	}
	
	void SpawnGuards() {
	}
	
	void SpawnWalls() {
	}
}
