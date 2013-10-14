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
	}
	
	void AddRoom(IntVector2 pos) {
	}
	
	void ConnectRoom(IntVector2 pos, IntVector2 dir) {
		if (dir.gridMagnitude > 1) {
		}
	}
}
