using UnityEngine;
using System.Collections.Generic;

public class RandomBag<T> : AbstractBag<T> {
	protected List<T> members;
	
	public override int count {
		get {
			return members.Count;
		}
	}

	public override T GetNext() {
		if (members.Count > 0) {
			int i = Random.Range(0, members.Count);
			T item = members[i];
			return item;
		} else {
			string msg = string.Format("{0} of type {1} is empty!", this.GetType().ToString(), typeof(T).ToString());
			Debug.LogWarning(msg);
			return default(T);
		}
	}


	public virtual void Add(T item) {
		members.Add(item);
	}

	public virtual bool Remove(T item) {
		return members.Remove(item);
	}


	public void RemoveRange(IEnumerable<T> items) {
		foreach (var item in items) {
			Remove(item);
		}
	}

	public void AddRange(IEnumerable<T> items) {
		foreach (var item in items) {
			Add(item);
		}
	}
	

	public RandomBag() {
		members = new List<T>();
	}

	public RandomBag(int capacity) {
		members = new List<T>(capacity);
	}

	public RandomBag(IEnumerable<T> items) : this() {
		AddRange(items);
	}
	
	public RandomBag(params T[] items) : this(items) {
	}
}
