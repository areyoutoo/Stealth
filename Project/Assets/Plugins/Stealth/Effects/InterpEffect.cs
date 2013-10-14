using UnityEngine;
using System.Collections;

public enum InterpEffectMode {
	SineUp,
	LineUp,
	SineThrob,
	LineThrob,
}

public abstract class InterpEffect : Effect {
	[SerializeField] protected float period = 1f;
	[SerializeField] protected InterpEffectMode mode = InterpEffectMode.SineThrob;
	
	public override void UpdateEffect() {
		base.UpdateEffect();
		float param = CalcInterp(effectTime);
		Interpolate(param);
	}
	
	protected abstract void Interpolate(float param);
	
	float CalcInterp(float param) {
		switch(mode) {
		case InterpEffectMode.LineUp:
			return Mathf.Repeat(param, period);
			
		case InterpEffectMode.LineThrob:
			return Mathf.PingPong(param, period);
			
		case InterpEffectMode.SineUp:
			float a = Mathf.Repeat(param / period, Mathf.PI * 0.5f);
			return Mathf.Sin(a);
			
		case InterpEffectMode.SineThrob:
			float t = Mathf.Repeat(param / period, Mathf.PI * 2f);
			float s = Mathf.Sin(t);
			return Mathf.InverseLerp(-1f, 1f, s);
			
		default:
			throw new System.NotImplementedException("InterpEffectMode " + mode);
		}
	}
}
