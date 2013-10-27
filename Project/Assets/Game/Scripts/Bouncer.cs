using UnityEngine;
using System.Collections;

public class Bouncer : MonoBehaviour {
	[SerializeField] float period = 1f;
	[SerializeField] Vector3 axis = Vector3.up;
	
	Vector3 basePos = Vector3.zero;
	
	protected void Update() {
		Vector3 offset = axis * Mathf.Sin(Time.time * period);
		transform.position = basePos + offset;
	}
	
	protected void OnEnable() {
		basePos = transform.position;
	}
}
