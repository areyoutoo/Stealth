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
		
		GameObject floor = (GameObject)Instantiate(barrierPrefab);
		GameObject ceiling = (GameObject)Instantiate(barrierPrefab);
		GameObject westWall = (GameObject)Instantiate(barrierPrefab);
		GameObject eastWall = (GameObject)Instantiate(barrierPrefab);
		
		floor.transform.localScale   = new Vector3(BARRIER_LENGTH, BARRIER_WIDTH, BARRIER_WIDTH);
		ceiling.transform.localScale = new Vector3(BARRIER_LENGTH, BARRIER_WIDTH, BARRIER_WIDTH);
		
		westWall.transform.localScale = new Vector3(BARRIER_WIDTH, BARRIER_LENGTH, BARRIER_WIDTH);
		eastWall.transform.localScale = new Vector3(BARRIER_WIDTH, BARRIER_LENGTH, BARRIER_WIDTH);
		
		Vector3 center = (coord * ROOM_SIZE).ToVector3(0f);
		
		floor.transform.position   = center + Vector3.up * HALF_ROOM_SIZE;
		ceiling.transform.position = center - Vector3.up * HALF_ROOM_SIZE;
		eastWall.transform.position = center + Vector3.right * HALF_ROOM_SIZE;
		westWall.transform.position = center - Vector3.right * HALF_ROOM_SIZE;
	}
}
