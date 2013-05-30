using UnityEngine;
using System.Collections;

public class RGGuard : MonoBehaviour {
	
	[SerializeField] GameObject bulletPrefab;
	
	CharacterController controller;
	
	Collider platform;
	
	const float SHOT_DELAY = 0.2f;
	
	const float MOVE_RATE = 1f;
	
	float timeToNextShot;
	
	Vector3 startPos;
	Vector3 endPos;
	
	Vector3 minPos;
	Vector3 maxPos;
	
	bool bMoving;
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
		
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f)) {
			platform = hit.collider;
			
			Vector3 newPos = hit.point;
			newPos.y = hit.collider.bounds.max.y + collider.bounds.size.y * 0.5f;
			transform.position = newPos;
		} else {
			Debug.Log("couldn't find floor: suiciding");
			Destroy(gameObject);
		}
		
		//TODO
		platform = collider;
		
		Bounds bounds = platform.bounds;
		minPos = new Vector3(bounds.min.x, bounds.max.y + collider.bounds.size.y * 0.5f, bounds.center.z);
		maxPos = new Vector3(bounds.max.x, bounds.max.y + collider.bounds.size.y * 0.5f, bounds.center.z);
		
		StopMoving();
	}
	
	void Update()
	{
		if (CanSeePlayer()) {
			timeToNextShot -= Time.deltaTime;
			if (timeToNextShot < 0f) {
				Shoot();
				timeToNextShot = SHOT_DELAY;
			}
		} else {
			timeToNextShot = 0f;
			
			if (bMoving) {
				controller.Move(transform.forward * MOVE_RATE);
			}
		}
	}
	
	void StartMoving()
	{
		startPos = transform.position;
		endPos = Vector3.Lerp(minPos, maxPos, Random.value);
		
		transform.LookAt(endPos);
		
		float delay = Vector3.Distance(startPos, endPos) / MOVE_RATE;
		Invoke ("StopMoving", delay);
		
		bMoving = true;
	}
	
	void StopMoving()
	{
		float param = Random.Range(Mathf.PI * 0.1f, Mathf.PI * 0.5f);
		float delay = Mathf.Sin(param);
		
		Invoke("StartMoving", delay);
		
		bMoving = false;
	}
	
	void Shoot()
	{
		//TODO
	}
	
	bool CanSeePlayer()
	{
		return Vector3.Angle(transform.forward, RGPlayer.instance.transform.position - transform.position) < 30f;
		//TODO
	}
}
