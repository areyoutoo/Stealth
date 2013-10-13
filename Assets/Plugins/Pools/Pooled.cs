using UnityEngine;
using System.Collections;

public class Pooled : MonoBehaviour {
	[SerializeField] int pool;
	public int poolID {
		get {
			return pool;
		}
	}
	
	Pool lastPool;
	public bool inPool { 
		get { 
			return lastPool != null; 
		} 
	}
	
	public void AddToPool(Pool pool) {
		lastPool = pool;
	}
	
	public void ReturnToPool() {
		OnReturnToPool();
		gameObject.SetActive(false);
		lastPool.Add(this);
	}
	
	protected virtual void OnReturnToPool() {
	}
	
	public void RemovedFromPool() {
		OnRemovedFromPool();
		gameObject.SetActive(true);
	}
	
	protected virtual void OnRemovedFromPool() {
	}
}
