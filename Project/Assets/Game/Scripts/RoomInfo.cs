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
	public Vector3 center { get; protected set; }
	
	
	public WeightedBag<string> pickupPrefabs;
	public WeightedBag<string> guardPrefabs;
	public ShuffleBag<Vector3> platformPositions;
	public ShuffleBag<Vector3> goldPositions;
	public ShuffleBag<Vector3> pickupPositions;
	
	public void Init(MapGen map, IntVector2 coord) {
		this.coord = coord;
		
		center = coord.ToVector3(0f) * (MapGen.ROOM_SIZE + MapGen.BARRIER_WIDTH);
		bounds = new Bounds(center, (Vector3.one * 2f * (MapGen.ROOM_SIZE + MapGen.BARRIER_WIDTH)).WithZ(0.1f));
		innerBounds = new Bounds(center, (Vector3.one * 1.8f * MapGen.ROOM_SIZE).WithZ(0.1f));
		
		guardPrefabs = new WeightedBag<string>();
		guardPrefabs.Add("Guard", 2f);
		guardPrefabs.Add("Flybot", 1f);
		
		pickupPrefabs = new WeightedBag<string>();
		pickupPrefabs.Add("ShurikenPickup", 1f);
		
		int xyl = Mathf.RoundToInt(MapGen.ROOM_SIZE)-1;
		Vector3 orig = Vector3.one.WithZ(0f) * MapGen.ROOM_SIZE;
		
		goldPositions = new ShuffleBag<UnityEngine.Vector3>(MakeGrid(orig, Vector3.right, Vector3.up, xyl, xyl));
		pickupPositions = new ShuffleBag<UnityEngine.Vector3>(MakeGrid(orig, Vector3.right, Vector3.up, xyl, xyl));
		platformPositions = new ShuffleBag<UnityEngine.Vector3>(MakeGrid(orig, Vector3.right*2, Vector3.up*4, xyl/2, xyl/4));
		
		SpawnPlatforms();
		SpawnGold();
		SpawnGuards();
		
		if (Random.value < 0.2f || coord == IntVector2.zero) {
			SpawnPickup();
		}
	}
	
	protected void Start() {

	}
	
	Vector3[] MakeGrid(Vector3 origin, Vector3 xs, Vector3 ys, int xl, int yl) {
		Vector3[] grid = new Vector3[xl * yl];
		int i=0;
		for (int x=0; x<xl; x++) {
			for (int y=0; y<yl; y++) {
				grid[i++] = (xs * x + ys * y) - origin;
			}
		}
		return grid;
	}
	
	
	
	void SpawnGold() {
		int count = Random.Range(5,10);
		for (int i=0; i<count; i++) {
			Vector3 pos = goldPositions.GetNext() + center;
			Transform gold = PoolManager.Get<TransformPool>("Gold").GetNextAt(pos);
			gold.parent = transform;
		}
	}
	
	void SpawnGuards() {
		if (coord == IntVector2.zero) return;
		
		int count = Random.Range(3, 6);
		for (int i=0; i<count; i++) {
			Vector3 pos = goldPositions.GetNext() + center;
			
			string poolName = guardPrefabs.GetNext();
			Transform guard = PoolManager.Get<TransformPool>(poolName).GetNextAt(pos, Quaternion.identity);			
			guard.parent = transform;
		}
	}
	
	void SpawnPlatforms() {
		platformPositions.Refill();
		
		int count = Random.Range(10, 15);
		for (int i=0; i<count; i++) {
			Vector3 pos = platformPositions.GetNext() + center;
			Transform platform = PoolManager.Get<TransformPool>("Platform").GetNextAt(pos);
			platform.parent = transform;
			
			Vector3 scale = new Vector3(Random.Range(0.5f, 2f), 1f, 1f);
			platform.localScale = Vector3.Scale(platform.transform.localScale, scale);
		}
	}
	
	void SpawnPickup() {
		string pool = pickupPrefabs.GetNext();
		Vector3 pos = pickupPositions.GetNext() + center;
		Transform pickup = PoolManager.Get<TransformPool>(pool).GetNextAt(pos, Quaternion.LookRotation(Vector3.right));
		pickup.parent = transform;
	}
}
