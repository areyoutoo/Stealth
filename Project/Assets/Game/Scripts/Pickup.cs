using UnityEngine;
using System.Collections;

public enum PickupType {
	None = 0,
	Shuriken = 10,
}

public class Pickup : MonoBehaviour {
	public PickupType type;
	public int count = 10;
	
	public void ReturnToPool() {
		if (type == PickupType.None) return;
		
		string pool = GetPoolName();
		PoolManager.Get<TransformPool>(pool).Add(transform);
	}
	
	public string GetPoolName() {
		switch (type) {
		case PickupType.Shuriken: return "ShurikenPickup";
		default: throw new System.NotImplementedException("Pickup.GetPoolName " + type);
		}
	}
}
