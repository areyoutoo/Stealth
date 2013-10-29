using UnityEngine;
using System.Collections;

public class ExplodingDeath : Death {
	
	[SerializeField] string explosionName;
	
	public override void Die() {
		base.Die();
	}
	
	protected override void OnFinishDying() {
		Vector3 pos = transform.position;
		base.OnFinishDying();
		
		PoolManager.Get<ParticlePool>(explosionName).GetNextAt(pos);
	}
}
