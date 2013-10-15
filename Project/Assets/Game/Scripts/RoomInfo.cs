﻿using UnityEngine;
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
	
	void SpawnGold() {
	}
	
	void SpawnGuards() {
	}
	
	void SpawnWalls() {
	}
}