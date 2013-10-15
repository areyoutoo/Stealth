using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {
	[SerializeField] int family;
	public int familyID {
		get {
			return family;
		}
	}
	
	Dictionary<int, Pool> poolMap;
	
	static Dictionary<int, PoolManager> familyMap;
	
	
	protected void Awake() {
		Register(familyID, this);
	}
	
	protected void OnDestroy() {
		Debug.LogWarning("foo");
		Unregister(familyID);
	}
	
	protected void Start() {
		RebuildPools();
	} 

	
	public void RebuildPools() {
		if (poolMap != null) {
			foreach (int id in poolMap.Keys) {
				Pool p = poolMap[id];
				p.Clear();
			}
			poolMap.Clear();
		} else {
			poolMap = new Dictionary<int, Pool>();
		}
		
		Pooled[] items = (Pooled[])GameObject.FindObjectsOfType(typeof(Pooled));
		foreach (Pooled item in items) {
			if (item.familyID == familyID && !item.inPool) {
				if (!poolMap.ContainsKey(item.poolID)) {
					poolMap.Add(item.poolID, new Pool(item.poolID));
				}
				poolMap[item.poolID].Add(item);
			}
		}
	}
	
	
	public GameObject Get(int poolID) {
		if (poolMap.ContainsKey(poolID)) {
			Pooled p = poolMap[poolID].GetNext();
			if (p != null) {
				return p.gameObject;
			} else {
				string msg = string.Format("Pool family {0} has no items in pool {1}", familyID, poolID);
				Debug.LogWarning(msg, this);
				return null;
			}
		} else {
			string msg = string.Format("Pool family {0} has no such pool {1}", familyID, poolID);
			Debug.LogWarning(msg, this);
			return null;
		}
	}
	
	public static void Register(int familyID, PoolManager manager) {
		if (familyMap.ContainsKey(familyID)) {
			string msg = string.Format("PoolManager duplicate family {0}", familyID);
			Debug.LogWarning(msg, manager);
		} else {
			familyMap.Add(familyID, manager);
		}
	}
	
	public static void Unregister(int familyID) {
		familyMap.Remove(familyID);
	}
	
	public static GameObject Get(int familyID, int poolID) {
		if (familyMap.ContainsKey(familyID)) {
			return familyMap[familyID].Get(poolID);
		} else {
			string msg = string.Format("PoolManager has no such family {0}", familyID);
			Debug.LogWarning(msg);
			return null;
		}
	}
	
	static PoolManager() {
		familyMap = new Dictionary<int, PoolManager>();
	}
}
