using UnityEngine;
using System.Collections.Generic;

public class MapGen : MonoBehaviour {
	RoomInfo currentRoom;
	List<RoomInfo> rooms;
	
	const float ROOM_SIZE = 10f;
	const float HALF_ROOM_SIZE = ROOM_SIZE / 2f;
	const float BARRIER_WIDTH = 1f;
	const float BARRIER_LENGTH = ROOM_SIZE + BARRIER_WIDTH;
	const float GAP_LENGTH = 4f;
	
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
		float far_left = center.x - HALF_ROOM_SIZE - BARRIER_WIDTH;
		float far_right = center.x + HALF_ROOM_SIZE + BARRIER_WIDTH;
		
		float gap_start = Random.Range(far_left + BARRIER_WIDTH, far_right - GAP_LENGTH - BARRIER_WIDTH);
		float gap_end = gap_start + GAP_LENGTH;
		
		Vector3 left_center = new Vector3(Mathf.Lerp(far_left, gap_start, 0.5f), center.y, center.z);
		Vector3 right_center = new Vector3(Mathf.Lerp(gap_end, far_right, 0.5f), center.y, center.z);
		
		Vector3 left_scale = new Vector3(gap_start - far_left, BARRIER_WIDTH, BARRIER_WIDTH);
		Vector3 right_scale = new Vector3(far_right - gap_end, BARRIER_WIDTH, BARRIER_WIDTH);
		
		SpawnBlock(left_center, left_scale);
		SpawnBlock(right_center, right_scale);
	}
	
	void SpawnWall(Vector3 center) {
		SpawnBlock(center, new Vector3(BARRIER_WIDTH, BARRIER_LENGTH, BARRIER_WIDTH));
	}
	
	void SpawnBlock(Vector3 center, Vector3 scale) {
		GameObject block = (GameObject)Instantiate(barrierPrefab);
		block.transform.localScale = scale;
		block.transform.position = center;
	}
}
