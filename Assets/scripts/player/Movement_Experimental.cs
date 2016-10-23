using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2DPhysics))]
[RequireComponent(typeof(SpriteRenderer))]
public class Movement_Experimental : MonoBehaviour {

	public float min_jump_height;
	public float max_jump_height;
	public float max_standing_jump_duration;
	//public float long_jump_ratio;

	public float horz_movement_speed;
	public float ground_acceleration;
	public float air_acceleration;

	public float decay_time, transition_time;

	public float horz_smoothing_time;

	float max_acceleration, min_acceleration, jump_time, initial_jump_velocity;
	float min_initial_jump_velocity;
	//these are basis vectors. Changing them will change the direction of the the entire physics script
	Vector2 up, right;

	bool jump_released;

	SpriteRenderer sprite;
	Controller2DPhysics physics;
	Controller2D controller;


	void Start () {
		max_acceleration = 2 * max_jump_height / (max_standing_jump_duration * max_standing_jump_duration);

		initial_jump_velocity = Mathf.Sqrt(2 * max_acceleration * max_jump_height);

		min_initial_jump_velocity = Mathf.Sqrt(2 * max_acceleration * min_jump_height);


		jump_time = 0;
		up = Vector2.up;
		right = Vector2.right;

		sprite = GetComponent<SpriteRenderer> ();
		physics = GetComponent<Controller2DPhysics> ();

		controller = GetComponent<Controller2D> ();
	}
	
	//this should really be a fixed update but it might be ok if it is an update instead
	void FixedUpdate () {
		//calcualte the change in time
		float delta = Time.deltaTime;
		//update the player velocity
		physics.Set_Velocity (Calculate_Horz_Velocity (delta) + Calculate_Vert_Velocity (delta), "Player_Input");

	}

	Vector2 Calculate_Vert_Velocity(float time){
		//get the extant physics data
		Vector2 velocity = physics.Get_Physics_Data("Player_Input").velocity;
		//filter by direction using a dot product
		//since the basis vectors are orthonormal this will give the projection of velocity onto
		//up
		float base_speed = velocity.x * up.x + velocity.y * up.y;
		velocity = physics.Get_Physics_Data("Max_Jump").velocity;
		float add_speed = velocity.x * up.x + velocity.y * up.y;
		//check to see if on the ground
		//if we like this script I will rewrite the script to account for a rotated basis
		//i.e. a change in gravity direction
		if (controller.collisions.below) {
			//reset this bool
			jump_released = false;
			//jump if the player presses jump
			if (Input.GetButton ("Jump")) {
				//jump_time = decay_time;
				base_speed = min_initial_jump_velocity;
				physics.Set_Velocity (up * (initial_jump_velocity - base_speed), "Max_Jump");
			} else {//don't jump if they didn't press jump
				physics.Set_Velocity (up * 0, "Max_Jump");
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
			base_speed -= time * max_acceleration;
		}
		//return the new data
		return up * base_speed;
	}

	Vector2 Calculate_Horz_Velocity(float time){
		Vector2 input = Input.GetAxisRaw("Horizontal") * right;
		if(input.x > 0)
		{
			sprite.flipX = true;
		}
		else if(input.x < 0)
		{
			sprite.flipX = false;
		}
		float speed = horz_movement_speed;
		return input * speed;
	}
}
