using UnityEngine;
using System.Collections.Generic;

public class MapGen : MonoBehaviour {
	RoomInfo currentRoom;
	List<RoomInfo> rooms;
	
	const float ROOM_SIZE = 10f;
	const float HALF_ROOM_SIZE = ROOM_SIZE / 2f;
	const float BARRIER_WIDTH = 1f;
	const float BARRIER_LENGTH = ROOM_SIZE + BARRIER_WIDTH;
	
	Dictionary<IntVector2, RoomInfo> roomMap;
	
	[SerializeField] GameObject barrierPrefab;
	[SerializeField] GameObject platformPrefab;
	
	protected void Awake() {
		rooms = new List<RoomInfo>();
		roomMap = new Dictionary<IntVector2, RoomInfo>();
	}
	
	protected void Start() {
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

		Vector3 center = (coord * ROOM_SIZE).ToVector3(0f);
		
		SpawnCeiling(center + Vector3.up * HALF_ROOM_SIZE);
		SpawnCeiling(center - Vector3.up * HALF_ROOM_SIZE);
		
		SpawnWall(center + Vector3.right * HALF_ROOM_SIZE);
		SpawnWall(center - Vector3.right * HALF_ROOM_SIZE);
	}
	
	void SpawnCeiling(Vector3 center) {
		GameObject ceiling = (GameObject)Instantiate(barrierPrefab);
		ceiling.transform.localScale = new Vector3(BARRIER_LENGTH, BARRIER_WIDTH, BARRIER_WIDTH);
		ceiling.transform.position = center;
	}
	
	void SpawnWall(Vector3 center) {
		GameObject wall = (GameObject)Instantiate(barrierPrefab);
		wall.transform.localScale = new Vector3(BARRIER_WIDTH, BARRIER_LENGTH, BARRIER_WIDTH);
		wall.transform.position = center;
	}
}
