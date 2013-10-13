using UnityEngine;
using System.Collections;

public class PoolManager : MonoBehaviour {
	[SerializeField] int family;
	public int familyID {
		get {
			return family;
		}
	}
	
	protected void Start() {
	}
}
