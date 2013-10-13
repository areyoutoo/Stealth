using UnityEngine;
using System.Collections;

public abstract class AbstractBag<T> {
	public abstract T GetNext();
	public abstract int count { get; }
}
