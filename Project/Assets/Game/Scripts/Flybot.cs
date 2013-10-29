using UnityEngine;
using System.Collections;

public class Flybot : Actor {
	
	protected void OnEnable() {
		InvokeRepeating("ChangeDirection", 0.1f, 1f);
	}

	protected void OnDisable() {
		CancelInvoke();
	}
	
	protected void Update() {
	}
	
	protected void ChangeDirection() {
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(Random.onUnitSphere.WithZ(0f), ForceMode.VelocityChange);
	}
	

}
