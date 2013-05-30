using UnityEngine;
using System.Collections.Generic;

public class RGPlayer : MonoBehaviour 
{
	CharacterController controller;
	
	Vector3 velocity;
	
	bool bWallCling;
	bool bHoldingJump;
	
	bool bReleasedJump;
	
	float currentJumpHoldTime;
	float currentWallJumpForgivenessTime;
	
	const float MAX_LIFETIME = 30f;
	
	float lifetime = MAX_LIFETIME;
	
	const float WALL_JUMP_FORGIVENESS_TIME = 0.2f;
	const float WALL_JUMP_SIDE_VELOCITY = 1.25f;
	
	const float JUMP_VELOCITY = 10f;
	const float JUMP_HOLD_TIME = 0.25f;
	
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
	
	int jumpThroughCheckMask;
	
	List<Collider> jumpThroughDisabledColliders;
	
	
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
			
			velocity.y = JUMP_VELOCITY;
			
			//if wall cling, "launch" away from the wall
			if (bWallCling) {
				if (bKeyLeft) {
					velocity.x = WALL_JUMP_SIDE_VELOCITY;
				} else if (bKeyRight) {
					velocity.x = -WALL_JUMP_SIDE_VELOCITY;
				}
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
		}
		if (bWallCling) {
			velocity.y = ApplyDrag(velocity.y, CLING_DRAG * Time.deltaTime, velocity.y < 0f ? MIN_CLING_SPEED : 0f);
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
		
	}
	
	void OnGUI(){
		GUILayout.Label(string.Format("{0:n2}", lifetime));
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
	}
}