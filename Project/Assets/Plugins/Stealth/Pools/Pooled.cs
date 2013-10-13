using UnityEngine;
using System.Collections;

public class Pooled : MonoBehaviour {
	[SerializeField] int pool;
	public int poolID {
		get {
			return pool;
		}
	}
	
	[SerializeField] int family;
	public int familyID {
		get {
			return family;
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
	
	public void ClearPool() {
		lastPool = null;
		gameObject.SetActive(true);
	}
	
	public void ReturnToPool() {
		if (lastPool != null) {
			OnReturnToPool();
			lastPool.Add(this);
		} else {
			Debug.LogWarning("No pool to return to!", this);
		}
		gameObject.SetActive(false);
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
