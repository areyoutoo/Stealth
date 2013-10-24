using UnityEngine;
using System.Collections.Generic;

public class RGPlayer : MonoBehaviour 
{
	
	[SerializeField] GameObject goldPrefab;
	[SerializeField] GameObject guardPrefab;
	
	[SerializeField] AudioSource jumpSound;
	[SerializeField] AudioSource goldSound;
	[SerializeField] AudioSource landSound;
	
	[SerializeField] ParticleSystem slideLeft;
	[SerializeField] ParticleSystem slideRight;
	[SerializeField] ParticleSystem dragLeft;
	[SerializeField] ParticleSystem dragRight;
	
	public static RGPlayer instance
	{
		get;
		private set;
	}
	
	CharacterController controller;
	
	Vector3 velocity;
	
	bool bWallCling;
	bool bHoldingJump;
	
	bool bReleasedJump;
	
	float currentJumpHoldTime;
	float currentWallJumpForgivenessTime;
	
	const int GUARD_COUNT = 5;
	
	const float MAX_LIFETIME = 30f;
	const float GOLD_LIFE_BONUS = 5f;
	
	float lifetime = MAX_LIFETIME;
	
	const float WALL_JUMP_FORGIVENESS_TIME = 0.2f;
	const float WALL_JUMP_SIDE_VELOCITY = 1.25f;
	
	const float JUMP_VELOCITY = 10f;
	const float JUMP_HOLD_TIME = 0.3f;
	
	const float RUN_ACCEL = 20f;
	const float FALL_ACCEL = -45f;
	
	const float CLING_DRAG = 70f;
	const float FLOOR_DRAG = 30f;
	const float AIR_DRAG = 5f;
	
	const float MIN_CLING_SPEED = 2f;
	const float MAX_FLOOR_SPEED = 10f;
	
	const float MAX_RUN_SPEED = 10f;
	
	const float AIR_CONTROL = 0.3f;
	
	const float MIN_JUMPTHROUGH_CHECK_DISTANCE = 2f;
	const float MAX_JUMPTHROUGH_VELOCITY = 1.1f;
	
	const int LAYER_GOLD = 13;
	const int LAYER_GUARD = 11;
	
	int jumpThroughCheckMask;
	
	bool wasGrounded;
	
	int currentGoldSpawned = 0;
	const int MAX_GOLD_SPAWNED = 5;
	const float GOLD_SPAWN_DELAY = GOLD_LIFE_BONUS * 0.8f;
	
	List<Collider> jumpThroughDisabledColliders;
	
	void Awake()
	{
		instance = this;
	}
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
		
		jumpThroughCheckMask |= 1 << LayerMask.NameToLayer("JumpThrough");
		jumpThroughDisabledColliders = new List<Collider>(20);
		
		SpawnGuards();
		
		for (int i=0; i<MAX_GOLD_SPAWNED; i++) {
			SpawnGold();
		}
		InvokeRepeating("SpawnGold", GOLD_SPAWN_DELAY, GOLD_SPAWN_DELAY);
	}
	
	void Update()
	{
		float upAccel = 0f;
		float sideAccel = 0f;
		
		bool bKeyLeft = Input.GetKey(KeyCode.A);
		bool bKeyRight = Input.GetKey(KeyCode.D);
		bool bKeyJump = Input.GetKey(KeyCode.Space);
		bool bKeyDown = Input.GetKey(KeyCode.S);
		
		//don't jump twice just by holding the button
		if (!bKeyJump) bReleasedJump = true;
		
		//running left and right
		if (bKeyLeft) sideAccel -= RUN_ACCEL;
		if (bKeyRight) sideAccel += RUN_ACCEL;

		//can we jump? checked in layers
		bool bCanJumpAtAll = bReleasedJump && !(bKeyLeft && bKeyRight);
		bool bCanJump = bCanJumpAtAll && (controller.isGrounded || bWallCling);
		
		//"jump forgiveness": we can still jump for a moment, as long as we could have jumped recently
		if (bCanJump) {
			currentWallJumpForgivenessTime = 0f;
		} else {
			currentWallJumpForgivenessTime += Time.deltaTime;
			if (currentWallJumpForgivenessTime < WALL_JUMP_FORGIVENESS_TIME) {
				bCanJump = bCanJumpAtAll;
			}
		}
		
		//initial jump code
		if (bCanJump && bKeyJump) {
			bHoldingJump = true;
			bReleasedJump = false;
			currentJumpHoldTime = 0f;
			currentWallJumpForgivenessTime = 1000f;
			
			//TODO
			wasGrounded = false;
			
			velocity.y = JUMP_VELOCITY;
			
			//if wall cling, "launch" away from the wall
			if (bWallCling) {
				if (bKeyLeft) {
					velocity.x = WALL_JUMP_SIDE_VELOCITY;
					PlayJumpEffect(transform.position, Vector3.right);
				} else if (bKeyRight) {
					velocity.x = -WALL_JUMP_SIDE_VELOCITY;
					PlayJumpEffect(transform.position, Vector3.left);
				}
			} else {
				PlayJumpEffect(transform.position, Vector3.up);
			}
		}
		
		//ongoing jump code
		if (controller.isGrounded) {
			bWallCling = false;
		} else {
			if (bHoldingJump) {			
				
				//player can hold jump key to jump higher
				currentJumpHoldTime += Time.deltaTime;
				bHoldingJump = bKeyJump && currentJumpHoldTime < JUMP_HOLD_TIME;
				if (velocity.y < JUMP_VELOCITY) {
					velocity.y = JUMP_VELOCITY;
				}
			}
			
			//air-based accel changes
			sideAccel *= AIR_CONTROL;
			upAccel = FALL_ACCEL;
		}
					
		//apply acceleration
		Vector3 acceleration = Vector3.up * upAccel + Vector3.right * sideAccel;
		velocity += acceleration * Time.deltaTime;
		velocity.x = Mathf.Clamp(velocity.x, -MAX_RUN_SPEED, MAX_RUN_SPEED);
		
		//apply drag for floors, walls
		if (!bKeyLeft && !bKeyRight) {
			float dragRate = controller.isGrounded ? FLOOR_DRAG : AIR_DRAG;
			velocity.x = ApplyDrag(velocity.x, dragRate * Time.deltaTime);
			if (controller.isGrounded) {
				if (velocity.x < -0.05f) {
					if (!dragLeft.isPlaying) {
						dragLeft.Emit(1);
						dragLeft.Play();
					}
				} else if (velocity.x > 0.05f) {
					if (!dragRight.isPlaying) {
						dragRight.Emit(1);
						dragRight.Play();
					}
				} else {
					dragLeft.Stop();
					dragRight.Stop();
				}
			} else {
				dragLeft.Stop();
				dragRight.Stop();
			}
		} else {
			dragLeft.Stop();
			dragRight.Stop();
		}
		
		
		if (bWallCling) {
			velocity.y = ApplyDrag(velocity.y, CLING_DRAG * Time.deltaTime, velocity.y < 0f ? MIN_CLING_SPEED : 0f);
			if (bKeyLeft) {
				if (!slideLeft.isPlaying) {
					slideLeft.Emit(1);
					slideLeft.Play();
				}
			} else if (bKeyRight) {
				if (!slideRight.isPlaying) {
					slideRight.Emit(1);
					slideRight.Play();
				}
			}
		} else {
			slideLeft.Stop();
			slideRight.Stop();
		}
		
		//apply velocity
		CollisionFlags moveCollisions = controller.Move(velocity * Time.deltaTime);
		
		//are we hitting the floor?
		if ((moveCollisions & CollisionFlags.Below) != 0) {
			velocity.y = 0f;
		}
		
		//are we hitting a wall?
		if ((moveCollisions & CollisionFlags.Sides) != 0) {
			bWallCling = !controller.isGrounded;
			velocity.x = 0f;
		} else {
			bWallCling = false;
		}
		
		//iterate recently disabled colliders; if we're above one, turn it back on
		for (int i=jumpThroughDisabledColliders.Count-1; i >=0; i--) {
			Collider c = jumpThroughDisabledColliders[i];
			if (c.bounds.max.y < collider.bounds.min.y) {
				SetJumpThrough(c, false);
				jumpThroughDisabledColliders.RemoveAt(i);
			}
		}
		
		//check nearby "JumpThrough" colliders
		//   - if we're below it, turn it off
		//   - if we're standing on it, player can press "down" to fall through
		//run an outer check, first: avoid allocating an enumerator every frame
		float jumpThroughCheckDistance = Mathf.Max(MIN_JUMPTHROUGH_CHECK_DISTANCE, Mathf.Abs(velocity.y));
		if (Physics.CheckSphere(transform.position, jumpThroughCheckDistance, jumpThroughCheckMask)) {
			foreach (var c in Physics.OverlapSphere(transform.position, jumpThroughCheckDistance, jumpThroughCheckMask)) {
				if (c.bounds.min.y > collider.bounds.max.y) {
					SetJumpThrough(c, true);
					jumpThroughDisabledColliders.Add(c);
				}
				
				if (velocity.y < MAX_JUMPTHROUGH_VELOCITY && bKeyDown && collider.bounds.min.y - c.bounds.max.y < 0.1f) {
					SetJumpThrough(c, true);
					jumpThroughDisabledColliders.Add(c);
				}
			}
		}
		
		//drain lifetime
		lifetime -= Time.deltaTime;
		if (lifetime < 0f) {
			Lose();
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel(0);
		}
		
		if (controller.isGrounded && !wasGrounded) {
			landSound.Play();
			wasGrounded = true;
		}
//		wasGrounded = controller.isGrounded;
	}
	
	void OnGUI(){
		string life = string.Format("{0:n2}", lifetime);
		
		GUI.contentColor = Color.yellow;
		GUI.backgroundColor = Color.yellow;
		
		GUILayout.BeginHorizontal();
		GUILayout.Label(life);
		GUILayout.Button("", GUILayout.Width(lifetime * 10f));
		GUILayout.EndHorizontal();
		
		const float MESSAGE_TIME = 5f;
		if (Time.timeSinceLevelLoad < MESSAGE_TIME) {
			GUILayout.Label("Collect gold to restore your lifespan!");
		}
	}
	
	float ApplyDrag(float inVelocity, float inDrag, float minVelocity = 0f)
	{
		bool bNegative = inVelocity < 0f;
		inVelocity = Mathf.Abs(inVelocity);
		inVelocity = Mathf.Max(inVelocity - inDrag, minVelocity);
		return bNegative ? -inVelocity : inVelocity;
	}
	
	void SetJumpThrough(Collider c, bool bCanJumpThrough)
	{
		int layer = LayerMask.NameToLayer(bCanJumpThrough ? "JumpThroughDisabled" : "JumpThrough");
		c.gameObject.layer = layer;
		Physics.IgnoreCollision(collider, c, bCanJumpThrough);
	}
	
	void Lose()
	{
		Debug.Log("You totally lose, bro");
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.layer)
		{
		case LAYER_GOLD:
			CollectGold(other.gameObject);
			break;
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		switch (hit.gameObject.layer) {
		case LAYER_GOLD:
			CollectGold(hit.gameObject);
			break;
		case LAYER_GUARD:
			Lose();
			break;
		default:
			break;
		}
	}
	
	void CollectGold(GameObject gold)
	{
		Pooled.ReturnOrDestroy(gold);
		
		var p = PoolManager.Get(300,2);
		p.GetComponent<PooledEmitter>().PlayAt(gold.transform.position);
		
		lifetime = Mathf.Min(MAX_LIFETIME, lifetime + GOLD_LIFE_BONUS);
		currentGoldSpawned -= 1;
		
		goldSound.Play();
	}
	
	void SpawnGold()
	{
		return;
		currentGoldSpawned += 1;
		
		var guards = (RGGuard[])Object.FindObjectsOfType(typeof(RGGuard));
		if (guards.Length < 1) return;
		
		var guard = guards[Random.Range(0, guards.Length-1)] as RGGuard;
		
		Vector3 pos = guard.transform.position + Vector3.up * Random.Range(0.25f, 2f);
		float offset = Random.Range(2f, 5f);
		if (Random.value < 0.5f) {
			offset *= -1f;
		}
		
		pos.x += offset;
		
		GameObject.Instantiate(goldPrefab, pos, Quaternion.identity);
	}
	
	void SpawnGuards()
	{
		var objects = (Platform[])Object.FindObjectsOfType(typeof(Platform));
		var platforms = new List<Platform>(objects);
		if (platforms.Count == 0) return;
		
		for (int i=0; i<GUARD_COUNT; i++) {
			var index = Random.Range(0, platforms.Count-1);
			var platform = platforms[index];
			platforms.RemoveAt(index);
			
			Vector3 pos = platform.collider.bounds.max;			
			float x = Mathf.Lerp(platform.collider.bounds.min.x, platform.collider.bounds.max.x, Random.Range(0.25f, 0.75f));
			pos.x = x;
			pos.z = platform.collider.bounds.center.z;
			GameObject.Instantiate(guardPrefab, pos, Quaternion.identity);
		}
	}
	
	
	
	void PlayLandEffect(Vector3 pos) {
	}
	
	void PlayJumpEffect(Vector3 pos, Vector3 dir) {
		jumpSound.Play();
		
		GameObject p = PoolManager.Get(300, 1);
		if (p != null) {
			p.transform.position = pos - dir.normalized * 0.5f;
			p.transform.LookAt(pos + dir + Vector3.up);
			p.particleSystem.Play();
		}
	}
}
