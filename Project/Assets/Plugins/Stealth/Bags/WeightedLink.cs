﻿using UnityEngine;
using System.Collections;

public class WeightedBagLink<T> {
	public readonly T target;
	public readonly float weight;
	
	public WeightedBagLink(T target, float weight) {
		if (weight < 0f) throw new System.ArgumentOutOfRangeException("weight");
		
		this.target = target;
		this.weight = weight;
	}
}
