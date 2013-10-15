using UnityEngine;
using System.Collections.Generic;

public class MapGen : MonoBehaviour {
	RoomInfo currentRoom;
	List<RoomInfo> rooms;
	
	public const float ROOM_SIZE = 20f;
	public const float HALF_ROOM_SIZE = ROOM_SIZE / 2f;
	public const float BARRIER_WIDTH = 1f;
	public const float BARRIER_LENGTH = ROOM_SIZE + BARRIER_WIDTH;
	public const float GAP_LENGTH = 6f;
	
	Dictionary<IntVector2, RoomInfo> roomMap;
	
	[SerializeField] GameObject barrierPrefab;
	[SerializeField] GameObject platformPrefab;
	
	public PoolManager poolManager { get; protected set; }
	
	GameObject root;
	
	protected void Awake() {
		rooms = new List<RoomInfo>();
		roomMap = new Dictionary<IntVector2, RoomInfo>();
		poolManager = GetComponent<PoolManager>();
	}
	
	protected void Start() {
		root = new GameObject("Rooms");
		
		ConnectRoom(IntVector2.zero, IntVector2.zero);
		ConnectRooms(IntVector2.zero);
		
		currentRoom = roomMap[IntVector2.zero];
	}
	
	protected void Update() {
		if (!currentRoom.bounds.Contains(transform.position)) {
			
			IntVector2[] dirs = new IntVector2[]{
				IntVector2.north,
				IntVector2.south,
				IntVector2.east,
				IntVector2.west,
			};
			
			bool found = false;
			foreach (IntVector2 dir in dirs) {
				IntVector2 c = currentRoom.coord + dir;
				if (roomMap.ContainsKey(c) && roomMap[c].bounds.Contains(transform.position)) {
					currentRoom = roomMap[c];
					found = true;
				}
			}
			
			if (!found) {
				Debug.LogWarning("Failed to locate room!");
			} else {
				ConnectRooms(currentRoom.coord);
				foreach (IntVector2 dir in dirs) {
					ConnectRooms(currentRoom.coord + dir);
				}
			}
		}
	}
	
	void ConnectRooms(IntVector2 pos) {
		ConnectRoom(pos, IntVector2.north);
		ConnectRoom(pos, IntVector2.south);
		ConnectRoom(pos, IntVector2.east);
		ConnectRoom(pos, IntVector2.west);
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
		
		RoomInfo room = parent.AddComponent<RoomInfo>();
		rooms.Add(room);
		roomMap.Add(coord, room);
		room.Init(this, coord);
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
		float far_left = center.y - HALF_ROOM_SIZE - BARRIER_WIDTH;
		float far_right = center.y + HALF_ROOM_SIZE + BARRIER_WIDTH;
		
		float gap_start = Random.Range(far_left + BARRIER_WIDTH, far_right - GAP_LENGTH - BARRIER_WIDTH);
		float gap_end = gap_start + GAP_LENGTH;
		
		Vector3 left_center = new Vector3(center.x, Mathf.Lerp(far_left, gap_start, 0.5f) + BARRIER_WIDTH*0.5f, center.z);
		Vector3 right_center = new Vector3(center.x, Mathf.Lerp(gap_end, far_right, 0.5f) - BARRIER_WIDTH*0.5f, center.z);
		
		Vector3 left_scale = new Vector3(BARRIER_WIDTH, gap_start - far_left, BARRIER_WIDTH);
		Vector3 right_scale = new Vector3(BARRIER_WIDTH, far_right - gap_end, BARRIER_WIDTH);
		
		SpawnBlock(parent, left_center, left_scale);
		SpawnBlock(parent, right_center, right_scale);
	}
	
	void SpawnBlock(GameObject parent, Vector3 center, Vector3 scale) {
		GameObject block = poolManager.Get(1);// (GameObject)Instantiate(barrierPrefab);
		block.transform.localScale = scale;
		block.transform.position = center;
		block.transform.parent = parent.transform;
	}
	
	public GameObject GetPlatformPrefab() {
		return platformPrefab;
	}
}
