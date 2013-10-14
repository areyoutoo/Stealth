using UnityEngine;
using System.Collections;

public class Throb : InterpEffect {
	[SerializeField] Vector3 peakSize = Vector3.one * 2f;
	[SerializeField] Vector3 valleySize = Vector3.one * 0.5f;
	[SerializeField] bool restoreSize;
	
	protected Vector3 originalSize;
	
	public override void StartEffect() {
		originalSize = transform.localScale;
		base.StartEffect();
	}
	
	public override void StopEffect() {
		if (restoreSize) {
			transform.localScale = originalSize;
		}
		base.StopEffect();
	}
	
	protected override void Interpolate (float param) {
		transform.localScale = Vector3.Lerp(valleySize, peakSize, param);
//		Debug.Log(param);
	}
}
