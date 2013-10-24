using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class PooledEmitter : Pooled {
	[SerializeField] bool playOnRemove = true;
	
	protected void Update() {
		if (!particleSystem.IsAlive()) {
			ReturnToPool();
		}
	}
	
	protected override void OnReturnToPool() {
		particleSystem.Stop();
	}
	
	protected override void OnRemovedFromPool() {
		base.OnRemovedFromPool();
		if (playOnRemove) {
			particleSystem.Play();
		}
	}
	
	public void PlayAt(Vector3 pos) {
		transform.position = pos;
		particleSystem.Play();
	}
}
