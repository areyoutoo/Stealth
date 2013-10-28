using UnityEngine;
using System.Collections;

public class PlayerDeath : Death {
	
	protected override void OnFinishDying() {
		
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = false;
		}
		
		Invoke("Reload", 1f);
		Debug.Log("PLAYER FINISHED DYING");
	}
	
	void Reload() {
		Application.LoadLevel(Application.loadedLevel);
	}
}
