using UnityEngine;
using System.Collections;

public class Throb : InterpEffect {
	[SerializeField] Vector3 peakSize = Vector3.one * 2f;
	[SerializeField] Vector3 valleySize = Vector3.one * 0.5f;
	[SerializeField] bool restoreSize;
	
	protected Vector3 maxSize = Vector3.one;
	protected Vector3 minSize = Vector3.one;
	
	protected Vector3 originalSize;
	
	public override void StartEffect() {
		base.StartEffect();
		originalSize = transform.localScale;
		minSize = Vector3.Scale(originalSize, valleySize);
		maxSize = Vector3.Scale(originalSize, peakSize);
	}
	
	public override void StopEffect() {
		if (restoreSize) {
			transform.localScale = originalSize;
		}
		base.StopEffect();
	}
	
	protected override void Interpolate (float param) {
		Vector3 v = Vector3.Lerp(minSize, maxSize, param);
		transform.localScale = v;
	}
}
