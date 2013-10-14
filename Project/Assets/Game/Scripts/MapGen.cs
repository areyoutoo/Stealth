using UnityEngine;
using System.Collections.Generic;

public class MapGen : MonoBehaviour {
	RoomInfo currentRoom;
	List<RoomInfo> rooms;
	
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
		
		floor.transform.localScale   = new Vector3(10f, 1f, 1f);
		ceiling.transform.localScale = new Vector3(10f, 1f, 1f);
		
		westWall.transform.localScale = new Vector3(1f, 10f, 1f);
		eastWall.transform.localScale = new Vector3(1f, 10f, 1f);
		
		Vector3 center = coord.ToVector3(0f);
		floor.transform.position   = center + Vector3.up * 5f;
		ceiling.transform.position = center - Vector3.up * 5f;
		eastWall.transform.position = center + Vector3.right * 5f;
		westWall.transform.position = center - Vector3.right * 5f;
	}
}
