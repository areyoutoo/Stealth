using UnityEngine;
using System.Collections;

public class FlybotDeath : Death {
	[SerializeField] string ragdollName;
	
	public override void Die() {
		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		OnFinishDying();
		PoolManager.Get<TransformPool>(ragdollName).GetNextAt(pos, rot);
	}
}
