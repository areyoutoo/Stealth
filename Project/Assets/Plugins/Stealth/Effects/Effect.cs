using UnityEngine;
using System.Collections;

public abstract class Effect : MonoBehaviour {
	[SerializeField] protected float duration = 1f;
	[SerializeField] protected bool playOnAwake = false;
	
	public bool effectActive { get; protected set; }
	public float effectTime { get; protected set; }
	
	protected virtual void Awake() {
		enabled = playOnAwake;
		if (playOnAwake) {
			StartEffect();
		}
	}
	
	public void Update() {
		effectTime += Time.deltaTime;
		if (duration == 0f || effectTime <= duration) {
			UpdateEffect();
		} else {
			StopEffect();
		}
	}
	
	public virtual void StartEffect() {
		effectTime = 0f;
		enabled = true;
	}
	
	public virtual void StopEffect() {
		enabled = false;
	}
	
	public virtual void UpdateEffect() {
	}
}
