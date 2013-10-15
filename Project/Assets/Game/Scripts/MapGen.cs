﻿using UnityEngine;
using System.Collections.Generic;

public class MapGen : MonoBehaviour {
	RoomInfo currentRoom;
	List<RoomInfo> rooms;
	
	const float ROOM_SIZE = 20f;
	const float HALF_ROOM_SIZE = ROOM_SIZE / 2f;
	const float BARRIER_WIDTH = 1f;
	const float BARRIER_LENGTH = ROOM_SIZE + BARRIER_WIDTH;
	const float GAP_LENGTH = 6f;
	
	Dictionary<IntVector2, RoomInfo> roomMap;
	
	[SerializeField] GameObject barrierPrefab;
	[SerializeField] GameObject platformPrefab;
	
	GameObject root;
	
	protected void Awake() {
		rooms = new List<RoomInfo>();
		roomMap = new Dictionary<IntVector2, RoomInfo>();
	}
	
	protected void Start() {
		root = new GameObject("Rooms");
		
		ConnectRoom(IntVector2.zero, IntVector2.zero);
		ConnectRoom(IntVector2.zero, IntVector2.north);
		ConnectRoom(IntVector2.zero, IntVector2.south);
		ConnectRoom(IntVector2.zero, IntVector2.east);
		ConnectRoom(IntVector2.zero, IntVector2.west);
	}
	
	void ConnectRoom(IntVector2 pos, IntVector2 dir) {
		if (dir.gridMagnitude > 1) {
			return;
		}
		
		IntVector2 coord = pos + dir;
		if (roomMap.ContainsKey(coord)) {
			return;
		}
		
		GameObject parent = new GameObject(coord.ToString());
		parent.transform.parent = root.transform;

		Vector3 center = (coord * ROOM_SIZE).ToVector3(0f);
		
		if (dir != IntVector2.south) {
			SpawnCeiling(parent, center + Vector3.up * HALF_ROOM_SIZE);
		}
		if (dir != IntVector2.north) {
			SpawnCeiling(parent, center - Vector3.up * HALF_ROOM_SIZE);
		}
		
		if (dir != IntVector2.west) {
			SpawnWall(parent, center + Vector3.right * HALF_ROOM_SIZE);
		}
		if (dir != IntVector2.east) {
			SpawnWall(parent, center - Vector3.right * HALF_ROOM_SIZE);
		}
	}
	
	void SpawnCeiling(GameObject parent, Vector3 center) {		
		float far_left = center.x - HALF_ROOM_SIZE - BARRIER_WIDTH;
		float far_right = center.x + HALF_ROOM_SIZE + BARRIER_WIDTH;
		
		float gap_start = Random.Range(far_left + BARRIER_WIDTH, far_right - GAP_LENGTH - BARRIER_WIDTH);
		float gap_end = gap_start + GAP_LENGTH;
		
		Vector3 left_center = new Vector3(Mathf.Lerp(far_left, gap_start, 0.5f) + BARRIER_WIDTH*0.5f, center.y, center.z);
		Vector3 right_center = new Vector3(Mathf.Lerp(gap_end, far_right, 0.5f) - BARRIER_WIDTH*0.5f, center.y, center.z);
		
		Vector3 left_scale = new Vector3(gap_start - far_left, BARRIER_WIDTH, BARRIER_WIDTH);
		Vector3 right_scale = new Vector3(far_right - gap_end, BARRIER_WIDTH, BARRIER_WIDTH);
		
		SpawnBlock(parent, left_center, left_scale);
		SpawnBlock(parent, right_center, right_scale);
	}
	
	void SpawnWall(GameObject parent, Vector3 center) {
		SpawnBlock(parent, center, new Vector3(BARRIER_WIDTH, BARRIER_LENGTH, BARRIER_WIDTH));
	}
	
	void SpawnBlock(GameObject parent, Vector3 center, Vector3 scale) {
		GameObject block = (GameObject)Instantiate(barrierPrefab);
		block.transform.localScale = scale;
		block.transform.position = center;
		block.transform.parent = parent.transform;
	}
}
