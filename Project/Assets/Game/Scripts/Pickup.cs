using UnityEngine;
using System.Collections;

public enum PickupType {
	Shuriken = 10,
}

public class Pickup : MonoBehaviour {
	public PickupType type;
	
	public void ReturnToPool() {
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
