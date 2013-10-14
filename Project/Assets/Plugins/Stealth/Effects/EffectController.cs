using UnityEngine;
using System.Collections;

public class EffectController : RecursiveEffect<Effect> {
	public override void StartEffect() {
		base.StartEffect();
		for (int i=0; i<children.Count; i++) {
			if (!children[i].effectActive) {
				children[i].StartEffect();
			}
		}
	}
	
	public override void StopEffect() {
		for (int i=0; i<children.Count; i++) {
			if (children[i].effectActive) {
				children[i].StopEffect();
			}
		}
		base.StopEffect ();
	}
	
	public override void UpdateEffect() {
		base.UpdateEffect();
		for (int i=0; i<children.Count; i++) {
			if (children[i].effectActive) {
				children[i].UpdateEffect();
			}
		}
	}
}
