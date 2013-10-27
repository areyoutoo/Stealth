using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {
	[SerializeField] float degreesPerSecond = 360f;
	[SerializeField] Vector3 axis = Vector3.right;
	[SerializeField] Space space = Space.World;
	
	protected void Update() {
		transform.Rotate(axis, degreesPerSecond * Time.deltaTime, space);
	}
}
