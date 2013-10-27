using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	[SerializeField] int health = 1;
	
	public virtual void Damage() {
		if (--health < 0) {
			Die();
		}
	}
	
	public virtual void Die() {
		PoolManager.Get<TransformPool>(gameObject.name).Add(transform);
	}
}
