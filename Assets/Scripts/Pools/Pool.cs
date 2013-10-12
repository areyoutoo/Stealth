using UnityEngine;
using System.Collections.Generic;

public class Pool {
	public int max { get; protected set; }
	public int min { get; protected set; }
	public int current {
		get {
			return 1; //TODO
		}
	}
}
