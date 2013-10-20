using UnityEngine;
using System.Collections.Generic;

public class Pool : RandomBag<Pooled> {
	public int max { get; protected set; }
	public int min { get; protected set; }
	
	public readonly int poolID;
	
	protected int nextSpawnCount = 1;
	
	protected Pooled backupMember;
	
	
	public override Pooled GetNext() {
		if (count > 0) {
			Pooled p = base.GetNext();
			Remove(p);
			return p;
		} else if (backupMember != null) {
			string msg = string.Format("Pool {0} exhausted, spawning {1} new members", poolID, nextSpawnCount);
			Debug.Log(msg);
			CloneBackup();
			return GetNext();
		} else {
			string msg = string.Format("Pool {0} exhausted, has no backup to clone!", poolID);
			Debug.LogWarning(msg);
			return null;
		}
	}
	
	
	public override void Add(Pooled item) {
		if (backupMember == null) {
			backupMember = item;
			CloneBackup();
		} else {
			base.Add(item);
		}
		item.AddToPool(this);
		UpdateCounts();	
	}
	
	public override bool Remove(Pooled item) {
		bool b;
		if (backupMember == item) {
			Debug.LogWarning("Removing backup object from pool " + poolID);
			backupMember = null;
			b = true;
		} else {
			b = base.Remove(item);
		}
		UpdateCounts();
		return b;
	}
	
	
	public void Clear() {
		if (backupMember != null) {
			backupMember.ClearPool();
			backupMember = null;
		}
		foreach (var p in members) {
			p.ClearPool();
		}
		members.Clear();
	}
	
	
	protected void CloneBackup() {
		GameObject original = backupMember.gameObject;
		for (int i=0; i<nextSpawnCount; i++) {
			GameObject clone = (GameObject)GameObject.Instantiate(original, original.transform.position, original.transform.rotation);
			Pooled p = clone.GetComponent<Pooled>();
			clone.transform.parent = backupMember.transform.parent;
			Add(p);
		}
		
		nextSpawnCount = Mathf.Min(nextSpawnCount * 2, 128);
	}
	
	
	protected void UpdateCounts() {
		if (max < count) max = count;
		if (min > count) min = count;
	}
	
	protected void ResetCounts() {
		max = int.MinValue;
		min = int.MaxValue;
	}
	
	
	public Pool(int id) : base() {
		this.poolID = id;
		ResetCounts();
	}
	
	public Pool(int id, int capacity) : base(capacity) {
		this.poolID = id;
		ResetCounts();
	}
	
	public Pool(int id, IEnumerable<Pooled> items) : this(id) {
		AddRange(items);
	}
	
	public Pool(int id, params Pooled[] items) : this(id) {
		AddRange(items);
	}
}
