using UnityEngine;
using System.Collections;

public class Timebomb : Actor {
	[SerializeField] float timeToDie = 3f;
	
	protected void OnEnable() {
		Invoke("Explode", timeToDie);
	}
	
	protected void OnDisable() {
		CancelInvoke();
	}
	
	public void Explode() {
		Die();
	}
}
