using UnityEngine;
using System.Collections;

public class Timebomb : Actor {
	[SerializeField] float timeToDie = 3f;
	
	float currentTimeToDie;
	
	protected void OnEnable() {
		currentTimeToDie = timeToDie;
	}
	
	protected void Update() {
		currentTimeToDie -= Time.deltaTime;
		if (currentTimeToDie < 0f) {
			Die();
		}
	}
	
	public void Explode() {
		Die();
	}
}
