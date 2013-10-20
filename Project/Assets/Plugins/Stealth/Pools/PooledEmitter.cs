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
		if (playOnRemove) {
			particleSystem.Play();
		}
	}
}
