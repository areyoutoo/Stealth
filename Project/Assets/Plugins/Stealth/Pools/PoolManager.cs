﻿using UnityEngine;
using System.Collections.Generic;

public static class PoolManager {
	static Dictionary<string, ComponentPoolBase> poolMap = new Dictionary<string, ComponentPoolBase>();
	
	public static void Add(ComponentPoolBase pool) {
		string id = pool.id;
		if (poolMap.ContainsKey(id)) {
			string msg = string.Format("Ignoring duplicate pool '{0}'", id);
			Debug.LogWarning(msg);
		} else {
			poolMap.Add(id, pool);
		}
	}
	
	public static void Remove(string id) {
		poolMap.Remove(id);
	}
	
	public static ComponentPoolBase Get(string id) {
		ComponentPoolBase pool;
		if (!poolMap.TryGetValue(id, out pool)) {
			string msg = string.Format("No such pool '{0}'", id);
			Debug.LogWarning(msg);
		}
		return pool;
	}
	
	public static T Get<T>(string id) where T : ComponentPoolBase {
		T pool = null;
		ComponentPoolBase bPool;
		if (!poolMap.TryGetValue(id, out bPool)) {
			string msg = string.Format("No such pool '{0}'", id);
			Debug.LogWarning(msg);
		} else {
			pool = bPool as T;
			if (pool == null) {
				string msg = string.Format("Pool '{0}' is not of type '{1}'", id, typeof(T));
				Debug.LogWarning(msg, bPool);
			}
		}
		return pool;
	}
}