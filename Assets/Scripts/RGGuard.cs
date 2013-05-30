using UnityEngine;
using System.Collections;

public class RGGuard : MonoBehaviour {
	
	[SerializeField] GameObject bulletPrefab;
	
	CharacterController controller;
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
	}
	
	void Update()
	{
		
	}
}
