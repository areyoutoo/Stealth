using UnityEngine;
using System.Collections.Generic;

public class ShuffleBag<T> : RandomBag<T> {
	protected List<T> backups;
	
	public override T GetNext ()
	{
		if (members.Count < 1) {
			members.AddRange(backups);
		}
		return base.GetNext ();
	}
	
	public override void Add(T item) {
		base.Add(item);
		backups.Add(item);
	}
	
	public override bool Remove(T item) {
		bool m = base.Remove(item);
		bool b = backups.Remove(item);
		return m || b;
	}
	
	public ShuffleBag() : base() {
		backups = new List<T>();
	}
	
	public ShuffleBag(int capacity) : base(capacity) {
		backups = new List<T>(capacity);
	}
	
	public ShuffleBag(IEnumerable<T> items) : this() {
		AddRange(items);
	}
	
	public ShuffleBag(params T[] items) : this(items) {
	}
}
