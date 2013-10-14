using UnityEngine;
using System.Collections;

public class Flicker : RendererEffect {
	[SerializeField] float blinkTime;
	
	protected float currentBlinkTime;
	protected bool currentBlink = true;
	
	public override void StartEffect() {
		base.StartEffect();
		SetBlink(false);
	}
	
	public override void StopEffect() {
		SetBlink(true);
		base.StopEffect();
	}
	
	public override void UpdateEffect() {
		base.UpdateEffect();
		currentBlinkTime += Time.deltaTime;
		if (currentBlinkTime <= blinkTime) {
			SetBlink(!currentBlink);
		}
	}
	
	protected void SetBlink(bool on) {
		currentBlinkTime = 0f;
		currentBlink = on;
		for (int i=0; i<children.Count; i++) {
			children[i].enabled = on;
		}
	}
}
