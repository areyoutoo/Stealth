using UnityEngine;
using System.Collections.Generic;

public abstract class RecursiveEffect<T> : Effect where T : Component {	
	protected List<T> children = new List<T>();
	
	protected override void Awake() {
		base.Awake();
		CacheChildren();
	}
	
	public void CacheChildren() {
		children.Clear();
		foreach (T child in GetComponentsInChildren<T>()) {
			if (child != this) {
				children.Add(child);
			}
		}
	}
}
