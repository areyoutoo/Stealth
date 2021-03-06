using UnityEngine;
using System.Collections.Generic;

public class RGPlayer : Actor
{
	[SerializeField] AudioSource jumpSound;
	[SerializeField] AudioSource goldSound;
	[SerializeField] AudioSource landSound;
	[SerializeField] AudioSource bladeSound;
	
	[SerializeField] ParticleSystem slideLeft;
	[SerializeField] ParticleSystem slideRight;
	[SerializeField] ParticleSystem dragLeft;
	[SerializeField] ParticleSystem dragRight;
	
	PickupType currentPickup = PickupType.Shuriken;
	int currentPickupCount = 10;
	
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
	const int LAYER_PICKUP = 15;
	
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
		
		bool bPlaySlideLeft = false;
		bool bPlaySlideRight = false;
		
		
		if (bWallCling) {
			velocity.y = ApplyDrag(velocity.y, CLING_DRAG * Time.deltaTime, velocity.y < 0f ? MIN_CLING_SPEED : 0f);
			if (bKeyLeft) {
				bPlaySlideLeft = true;
			} else if (bKeyRight) {
				bPlaySlideRight = true;
			}
		}
		
		//apply velocity
		CollisionFlags moveCollisions = controller.Move(velocity * Time.deltaTime);
		
		//are we hitting the floor?
		if ((moveCollisions & CollisionFlags.Below) != 0) {
			velocity.y = 0f;
			bPlaySlideLeft = false;
			bPlaySlideRight = false;
			//TODO: do something about landing checks here?
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
		
		if (bPlaySlideLeft && !slideLeft.isPlaying) {
			slideLeft.Emit(1);
			slideLeft.Play();
		} else if (!bPlaySlideLeft && slideLeft.isPlaying) {
			slideLeft.Stop();
		}
		
		if (bPlaySlideRight && !slideRight.isPlaying) {
			slideRight.Emit(1);
			slideRight.Play();
		} else if (!bPlaySlideRight && slideRight.isPlaying) {
			slideRight.Stop();
		}
		
		//drain lifetime
		lifetime -= Time.deltaTime;
		if (lifetime < 0f) {
			Die();
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel(0);
		}
		
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl)) {
			UsePickup();
		}
		
		//TODO
		if (Input.GetKeyDown(KeyCode.B)) {
			currentPickup = PickupType.Shuriken;
			currentPickupCount = 100;
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
		
		//TODO
		switch (currentPickup) {
		case PickupType.None: 
			break;
		case PickupType.Shuriken:
			GUILayout.Label("Shurikens: " + currentPickupCount); 
			break;
		default:
			throw new System.NotImplementedException("RGPlayer.OnGUI " + currentPickup);
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
	
//	void Lose()
//	{
//		Debug.Log("You totally lose, bro");
//		Application.LoadLevel(Application.loadedLevel);
//	}
	
	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.layer)
		{
		case LAYER_GOLD:
			CollectGold(other.gameObject);
			break;
		case LAYER_PICKUP:
			CollectPickup(other.gameObject);
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
			Damage();
			break;
		default:
			break;
		}
	}
	
	void CollectGold(GameObject gold)
	{
		PoolManager.Get<ParticlePool>("GoldPoof").GetNextAt(gold.transform.position);
		PoolManager.Get<TransformPool>("Gold").Add(gold.transform);
		
		lifetime = Mathf.Min(MAX_LIFETIME, lifetime + GOLD_LIFE_BONUS);
		currentGoldSpawned -= 1;
		
		goldSound.Play();
	}
	
	void CollectPickup(GameObject pickup) {
		PoolManager.Get<ParticlePool>("PickupPoof").GetNextAt(pickup.transform.position);
		
		
		
		Pickup p = pickup.GetComponent<Pickup>();
		currentPickup = p.type;
		currentPickupCount = p.count;
		
		//TODO: do something with pickup type?
		
		//TODO: better sound?
		bladeSound.Play();
		
		p.ReturnToPool();
	}
	
	void UsePickup() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition.WithZ(0f)).WithZ(transform.position.z);
		Vector3 dir = mousePos - transform.position;
		Vector3 pos;
		Transform item;
		
		switch (currentPickup) {
		case PickupType.None: 
			return;
			
		case PickupType.Shuriken:
			pos = transform.position + dir.WithLength(1.5f);
			item = PoolManager.Get<TransformPool>("Shuriken").GetNextAt(pos, Quaternion.LookRotation(Vector3.right));
			item.GetComponent<Shuriken>().FirstLaunch(dir, 3);
			break;
			
		default: throw new System.NotImplementedException("RGPlayer.UsePickup " + currentPickup);
		}
		
		currentPickupCount -= 1;
		if (currentPickupCount < 1) {
			currentPickup = PickupType.None;
		}
	}
	
	void PlayLandEffect(Vector3 pos) {
	}
	
	void PlayJumpEffect(Vector3 pos, Vector3 dir) {
		jumpSound.Play();
		
		Quaternion rot = Quaternion.LookRotation(dir);
		pos = pos - dir.normalized * 0.5f;
		PoolManager.Get<ParticlePool>("JumpPoof").GetNextAt(pos, rot);
	}
	
//	protected override void Die () {
//		//TODO
//		enabled = false;
//		Invoke("Lose", 1.2f);
//	}
}
