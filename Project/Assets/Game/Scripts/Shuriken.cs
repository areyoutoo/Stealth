using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Shuriken : MonoBehaviour {
	int bounces;
	
	Vector3 launchDir;
	
	int killLayers;
	
	public void FirstLaunch(Vector3 dir, int inBounces) {
		bounces = inBounces;
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
			
			string bloodParticle = "Blood";
			
			Actor a = collision.gameObject.GetComponent<Actor>();
			if (a != null) {
				a.Damage();
				bloodParticle = a.bloodParticles;
			} else {
				Destroy(collision.gameObject);
			}
			
			Quaternion rot = Quaternion.LookRotation(collision.contacts[0].normal);
			PoolManager.Get<ParticlePool>(bloodParticle).GetNextAt(transform.position, rot);
			
			Shatter();
		} else if (--bounces > 0) {
			Debug.Log("Shuriken bounce " + bounces);
			Vector3 normal = collision.contacts[0].normal;
			Launch(Vector3.Reflect(launchDir, normal));
		} else {
			Debug.Log("Shuriken shatter");
			Shatter();
		}
	}
	
	void Shatter() {
		PoolManager.Get<TransformPool>(gameObject.name).Add(transform);
	}
	
	protected void Awake() {
		killLayers = 0
			| 1 << LayerMask.NameToLayer("Player")
			| 1 << LayerMask.NameToLayer("Guard");
	}
}
