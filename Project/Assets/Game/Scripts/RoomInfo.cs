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
	
	MapGen mapGen;
	
	public void Init(MapGen map, IntVector2 coord) {
		this.coord = coord;
		mapGen = map;
		
		Vector3 center = coord.ToVector3(0f) * MapGen.ROOM_SIZE;
		bounds = new Bounds(center, (Vector3.one * 2f * (MapGen.ROOM_SIZE + MapGen.BARRIER_WIDTH)).WithZ(0.1f));
		innerBounds = new Bounds(center, (Vector3.one * 2f * MapGen.ROOM_SIZE).WithZ(0.1f));
		
		SpawnPlatforms();
		SpawnGold();
	}
	
	protected void Start() {
	}
	
	void SpawnGold() {
		int count = Random.Range(5,10);
		for (int i=0; i<count; i++) {
			GameObject gold = mapGen.poolManager.Get(300);
			
			gold.transform.position = Randomx.InBounds(innerBounds).WithZ(0f);
			gold.transform.parent = transform;
		}
	}
	
	void SpawnGuards() {
	}
	
	void SpawnPlatforms() {
		int count = Random.Range(10, 15);
		for (int i=0; i<count; i++) {
			GameObject platform = mapGen.poolManager.Get(2);
			
			Vector3 v = Randomx.InBounds(innerBounds).WithZ(0f);
			platform.transform.position = v;
			
			Vector3 scale = new Vector3(Random.Range(0.5f, 2f), 1f, 1f);
			platform.transform.localScale = Vector3.Scale(platform.transform.localScale, scale);
			
			platform.transform.parent = transform;
		}
	}
}
