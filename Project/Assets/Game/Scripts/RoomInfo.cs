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
	}
	
	protected void Start() {
		Bounds b = new Bounds(transform.position, Vector3.zero);
		foreach (Collider c in GetComponentsInChildren<Collider>()) {
			b.Encapsulate(c.bounds);
		}
		bounds = b;
		innerBounds = new Bounds(transform.position, b.size - Vector3.one - Vector3.one);
		
		SpawnPlatforms();
	}
	
	void SpawnGold() {
	}
	
	void SpawnGuards() {
	}
	
	void SpawnPlatforms() {
		int count = Random.Range(10, 15);
		for (int i=0; i<count; i++) {
			GameObject platform = (GameObject)Instantiate(mapGen.GetPlatformPrefab());
			
//			platform.transform.position = transform.position + Randomx.InBounds(innerBounds);
			Vector3 v = Randomx.InBounds(innerBounds);
			v.z = 0f;
			platform.transform.position = v;
			
			Vector3 scale = new Vector3(Random.Range(0.5f, 2f), 1f, 1f);
			platform.transform.localScale = Vector3.Scale(platform.transform.localScale, scale);
			
			platform.transform.parent = transform;
		}
	}
}
