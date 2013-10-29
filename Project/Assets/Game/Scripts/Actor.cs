using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	[SerializeField] int health = 1;
	[SerializeField] string _bloodParticles = "Blood";
	
	public string bloodParticles {
		get { return _bloodParticles; }
	}
	
	public virtual void Damage() {
		if (--health < 1) {
			Die();
		}
	}
	
	protected virtual void Die() {
		enabled = false;
		GetComponent<Death>().Die();
	}
}
