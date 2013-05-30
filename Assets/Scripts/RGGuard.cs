using UnityEngine;
using System.Collections;

public class RGGuard : MonoBehaviour {
	CharacterController controller;
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
	}
}
