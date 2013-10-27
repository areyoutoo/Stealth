using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Shuriken : MonoBehaviour {
	int bounces;
	
	Vector3 launchDir;
	
	static int killLayers;
	
	public void FirstLaunch(Vector3 dir) {
		bounces = 2;
		rigidbody.velocity = Vector3.zero;
		Launch(dir);
	}
	
	public void Launch(Vector3 dir) {
		launchDir = dir.WithLength(10f);
		rigidbody.AddForce(launchDir, ForceMode.VelocityChange);
	}
	
	protected void OnCollisionEnter(Collision collision) {
		int layerBit = 1 << collision.gameObject.layer;
		
		if ((layerBit & killLayers) != 0) {
			Debug.Log("Shuriken kill!");
			Actor a = collision.gameObject.GetComponent<Actor>();
			if (a != null) {
				a.Damage();
			} else {
				Destroy(collision.gameObject);
			}
			
			Quaternion rot = Quaternion.LookRotation(collision.contacts[0].normal);
			PoolManager.Get<ParticlePool>("Blood").GetNextAt(transform.position, rot);
			
			Shatter();
		} else if (bounces-- > 0) {
			Debug.Log("Shuriken bounce " + bounces);
			Vector3 normal = collision.contacts[0].normal;
			Launch(Vector3.Reflect(launchDir, normal));
		} else {
			Debug.Log("Shuriken shatter");
			Shatter();
		}
	}
	
	void Shatter() {
		PoolManager.Get<TransformPool>("Shuriken").Add(transform);
	}
	
	static Shuriken() {
		killLayers = 0
			| 1 << LayerMask.NameToLayer("Player")
			| 1 << LayerMask.NameToLayer("Guard");
	}
}
