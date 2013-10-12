using UnityEngine;
using System.Collections;

public class Pooled : MonoBehaviour {
	[SerializeField] int pool;
	
	protected void Start() {
	}
	
	public void ReturnToPool() {
	}
	
	protected virtual void OnReturnToPool() {
	}
	
	public void RemovedFromPool() {
	}
	
	protected virtual void OnRemovedFromPool() {
	}
}
