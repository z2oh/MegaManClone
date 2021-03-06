using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2DPhysics))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Gravity))]
public class Player_Movement : MonoBehaviour {

	public float min_jump_height;
	public float max_jump_height;
	public float max_standing_jump_duration;

	public float horz_movement_speed;

	public float decay_time, transition_time;

	float acceleration, max_jump_velocity;
	float min_jump_velocity;

	bool jump_released, facing_right;

	SpriteRenderer sprite;
	Controller2DPhysics physics;
	Controller2D controller;

	void Awake () {
		sprite = GetComponent<SpriteRenderer> ();
		physics = GetComponent<Controller2DPhysics> ();
		controller = GetComponent<Controller2D> ();
	}

	void Start () {
		acceleration = 2 * max_jump_height / (max_standing_jump_duration * max_standing_jump_duration);

		max_jump_velocity = Mathf.Sqrt(2 * acceleration * max_jump_height);

		min_jump_velocity = Mathf.Sqrt(2 * acceleration * min_jump_height);

		facing_right = true;
	}
	
	//this should really be a fixed update but it might be ok if it is an update instead
	void FixedUpdate () {
		//calcualte the change in time
		float delta = Time.deltaTime;
		//update the player velocity
		physics.Set_Velocity (Calculate_Horz_Velocity (delta) + Calculate_Vert_Velocity (delta), "Player_Input");

	}

	Vector3 Calculate_Vert_Velocity(float time){
		//get the extant physics data
		Vector3 velocity = physics.Get_Physics_Data("Player_Input").velocity;
		//filter by direction using a dot product
		//since the basis vectors are orthonormal this will give the projection of velocity onto
		//up
		float base_speed = velocity.x * Gravity.up.x + velocity.y * Gravity.up.y;
		velocity = physics.Get_Physics_Data("Max_Jump").velocity;
		float add_speed = velocity.x * Gravity.up.x + velocity.y * Gravity.up.y;
		//check to see if on the ground
		//if we like this script I will rewrite the script to account for a rotated basis
		//i.e. a change in gravity direction
		if (Test_Direction(-1 * Gravity.up)) {
			//reset this bool
			jump_released = false;
			//jump if the player presses jump
			if (Input.GetButton ("Jump")) {
				//jump_time = decay_time;
				base_speed = min_jump_velocity;
				physics.Set_Velocity (Gravity.up * (max_jump_velocity - base_speed), "Max_Jump");
			} else {//don't jump if they didn't press jump
				physics.Set_Velocity (Gravity.up * 0, "Max_Jump");
				base_speed = 0;
			}
		} else {//if in the air
			if (base_speed + add_speed > 0) {// if the player is moving up
				if (!Input.GetButton ("Jump")) {
					jump_released = true;
				}//if the player releases jump cut the jump short
				if (jump_released){
					physics.Kill(transition_time, "Max_Jump");
				}
			} else {//if the player is falling
				physics.Kill( decay_time, "Max_Jump"); // clear out any remaining jump data
			}
			//accelerate downward
			base_speed -= time * acceleration;
		}
		//return the new data
		return Gravity.up * base_speed;
	}

	Vector3 Calculate_Horz_Velocity(float time){
		Vector3 input = Input.GetAxisRaw("Horizontal") * Gravity.right;
		if(input.x > 0)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
		else if(input.x < 0)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
		sprite.flipX = facing_right;
		float speed = horz_movement_speed;
		return input * speed;
	}

	public bool Test_Direction (Vector3 dir){
		if (dir.y > 0)
			return controller.collisions.above;
		if (dir.y < 0)
			return controller.collisions.below;
		if (dir.x > 0)
			return controller.collisions.right;
		if (dir.x < 0)
			return controller.collisions.left;
		return false;
	}

	public Vector3 Get_Forward(){
		if (facing_right)
			return Gravity.right;
		return Gravity.right * -1;
	}
	public Vector3 Get_Up(){
		return Gravity.up;
	}
}
