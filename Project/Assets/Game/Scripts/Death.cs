using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {
	
	public bool isDying { get; protected set; }
	
	Vector3 scale;
	
	const float DEATH_TIME = 0.25f;
	float currentDeathTime;
	
	public virtual void Die() {
		isDying = true;
		currentDeathTime = 0f;
		Debug.Log("dying " + name, gameObject);
	}
	
	protected void Awake() {
		scale = transform.localScale;
	}
	
	protected virtual void Update() {
		if (isDying) {
			currentDeathTime += Time.deltaTime;
			if (currentDeathTime > DEATH_TIME) {
				isDying = false;
				OnFinishDying();
			} else {
				transform.localScale = scale.WithY(Mathf.Lerp(scale.y, 0f, currentDeathTime / DEATH_TIME));
			}
		}
	}
	
	protected virtual void OnFinishDying() {
		PoolManager.Get<TransformPool>(gameObject.name).Add(transform);
	}
}
