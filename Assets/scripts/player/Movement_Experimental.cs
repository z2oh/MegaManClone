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

	public float vert_velocity_cap;

	float horz_smoothing;
	public float horz_smoothing_time;

	float max_acceleration, min_acceleration, jump_time, initial_jump_velocity;
	Vector2 up, right;

	bool jump_released;

	SpriteRenderer sprite;
	Controller2DPhysics physics;
	Controller2D controller;
	// Use this for initialization
	void Start () {
		max_acceleration = 2 * max_jump_height / (max_standing_jump_duration * max_standing_jump_duration);
		initial_jump_velocity = 2 * max_jump_height / max_standing_jump_duration;
		min_acceleration = initial_jump_velocity * initial_jump_velocity / (2 * min_jump_height);

		jump_time = max_standing_jump_duration;
		up = Vector2.up;
		right = Vector2.right;

		horz_smoothing = 0f;

		sprite = GetComponent<SpriteRenderer> ();
		physics = GetComponent<Controller2DPhysics> ();
		//physics.Set_Velocity (new Vector2 (0, 0), "Player_Input");
		controller = GetComponent<Controller2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		float delta = Time.deltaTime;

		physics.Set_Velocity (Calculate_Horz_Velocity (delta) + Calculate_Vert_Velocity (delta), "Player_Input");

	}

	Vector2 Calculate_Vert_Velocity(float time){
		Vector2 velocity = physics.Get_Physics_Data ("Player_Input").velocity;
		float speed = velocity.x * up.x + velocity.y * up.y;
		if (controller.collisions.below) {
			jump_released = false;
			jump_time = max_standing_jump_duration;

			if (Input.GetButton ("Jump")) {
				speed = initial_jump_velocity;
			}
		} else {
			if (speed > 0) {
				if (!Input.GetButton ("Jump"))
					jump_released = true;
				if (jump_released) {
					speed -= time * min_acceleration;
				} else {
					speed -= time * max_acceleration;
					jump_time -= time;
				}
			} else {
				if (jump_time > 0) {
					speed -= time * max_acceleration;
					jump_time -= time;
				} else
					speed -= time * min_acceleration;
			}
		}
		if (speed < -vert_velocity_cap)
			speed = -vert_velocity_cap;
		return up * speed;
	}
	Vector2 Calculate_Horz_Velocity(float time){
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector2 velocity = physics.Get_Physics_Data ("Player_Input").velocity;
		float speed = velocity.x * right.x + velocity.y * right.y;
		if(input.x > 0)
		{
			sprite.flipX = true;
		}
		else if(input.x < 0)
		{
			sprite.flipX = false;
		}
		speed = Mathf.SmoothDamp(speed, input.x * horz_movement_speed, ref horz_smoothing, (controller.collisions.below) ? ground_acceleration : air_acceleration);
		return right * speed;
	}
}
