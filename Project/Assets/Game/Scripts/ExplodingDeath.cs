using UnityEngine;
using System.Collections;

public class ExplodingDeath : Death {
	
	[SerializeField] string explosionName;
	
	public override void Die() {
		OnFinishDying();
	}
	
	protected override void OnFinishDying() {
		Vector3 pos = transform.position;
		base.OnFinishDying();
		
		PoolManager.Get<ParticlePool>(explosionName).GetNextAt(pos);
	}
}
