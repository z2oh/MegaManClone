using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Controller2DPhysicsLight : MonoBehaviour {

	// The current velocity of the object
	//changing this value will NOT change the velocity of the object
	//Use the set velocity method instead
	[HideInInspector]
	public Vector3 velocity;

	//stored physics info
	Physics_Data physics;

	//The Controller2D that this script uses
	Controller2D controller;

	//These values are used by the lock velocity method
	//min is the minimum allowed velocity and max is the maximum allowed velocity
	//lock duration determines how long the system is locked
	Vector3 min, max;
	float lock_duration;

	void Awake(){
		controller = GetComponent<Controller2D> ();
	}
	void Start () {
		lock_duration = 0;

		//Create the default physics set
		physics = new Physics_Data ();
		physics.Reset ();
	}

	// This method needs to be a fixed update
	// There really isn't anyway around that
	void FixedUpdate () {
		//the change in time
		//as this is a fixed update this value is a constant
		float delta = Time.deltaTime;

		//the kill method allows you to tell a physics set to return to initial values
		//this reduces the remaining time stored in the Physics_Data struct
		if (physics.dying)
			physics.time_remaining -= delta;
		//if the physics data set is dead it needs to be returened to default values
		//as it will likely be used again it is kept in the dictionary
		if (physics.dying && physics.time_remaining <= 0)//Physics set is dead
			physics.Reset ();
		else{//Physics set is alive
			//accelerate
			physics.velocity += delta * physics.acceleration - physics.velocity * physics.drag;
			//reset the acceleration vector
			physics.acceleration = new Vector3 (0, 0, 0);
							//enforce the physics locks
			if (lock_duration > 0) {
				if (physics.velocity.x < min.x)
					physics.velocity = new Vector3 (min.x, physics.velocity.y, 0);
				if (physics.velocity.x > max.x)
					physics.velocity = new Vector3 (max.x, physics.velocity.y, 0);
				if (physics.velocity.y < min.y)
					physics.velocity = new Vector3 (physics.velocity.x, min.y, 0);
				if (physics.velocity.y > max.y)
					physics.velocity = new Vector3 (physics.velocity.x, max.y, 0);
			}
			//enforce collisions
			if (controller.collisions.left && physics.velocity.x < 0)
				physics.velocity = new Vector3 (0, physics.velocity.y, 0);
			if(controller.collisions.right && physics.velocity.x > 0)
				physics.velocity = new Vector3 (0, physics.velocity.y, 0);
			if (controller.collisions.below && physics.velocity.y < 0)
				physics.velocity = new Vector3 (physics.velocity.x, 0, 0);
			if(controller.collisions.above && physics.velocity.y > 0)
				physics.velocity = new Vector3 (physics.velocity.x, 0, 0);
			//update the net velocity
			//if the physics set is dying linearly decay to zero velocity
			}
		//decreament the lock time
		if(lock_duration > 0)
			lock_duration -= delta;
		//move the object
		controller.Move (delta * physics.velocity);
	}
	//This method lock the movement of the objcet to a specific range
	//this will override any  existing physics set but not the boundries
	//DO NOT use this method yet. it is a work in progress
	public void Force_Velocity(Vector3 v1, Vector3 v2, float time){
		min = new Vector3 ((v1.x > v2.x) ? v2.x : v1.x, (v1.y > v2.y) ? v2.y : v1.y, 0);
		max = new Vector3 ((v1.x > v2.x) ? v1.x : v2.x, (v1.y > v2.y) ? v1.y : v2.y, 0);
		lock_duration = time;
	}

	//Sets the porperties of a physics set. By default modifies the DEFAULT set
	//by default awakens the set accesssed
	public void Set_Velocity (Vector3 value, bool revive = true){
		if (revive)
			physics.dying = false;
		physics.velocity = value;
	}
	public void Accelerate (Vector3 value, bool revive = true){
		if (revive)
			physics.dying = false;
		physics.acceleration += value;
	}
	public void Set_Drag (float value, bool revive = true){
		if (revive)
			physics.dying = false;
		physics.drag = value;
	}
	//Kills the named physics set.
	//Default cannot be killed
	//If the set is already dying when this is called the 
	public void Kill (float time, string name){
		//default cannot be killed
		//if already dying
		if (physics.dying) {
			//keep dying at the new rate
			if (physics.decay_time > 0) {//this is if it was dying slowly
				float deathRatio = physics.time_remaining / physics.decay_time;
				physics.time_remaining = deathRatio * time;
			} else //in this case the death is instant
				physics.Reset();
		} else //otherwise start dying
			physics.time_remaining = time;

		physics.decay_time = time;
		//indicate that the set is dying
		physics.dying = true;
	}

	//returns the named physics set
	//if the name is not found creates a new set
	public Physics_Data Get_Physics_Data(){
		return physics;
	}

	public void Lock_Velocity(Vector3 v1, Vector3 v2, float time){
		min = new Vector3 (v1.x < v2.x ? v1.x : v2.y, v1.y < v2.y ? v1.y : v2.y, 0);
		max = new Vector3 (v1.x > v2.x ? v1.x : v2.y, v1.y > v2.y ? v1.y : v2.y, 0);
		lock_duration = time;
	}

	//The physics data struct
	public struct Physics_Data
	{
		//the velocity of the set
		public Vector3 velocity;
		//acceleration of the set
		public Vector3 acceleration;
		//linear drag
		public float drag;
		//date used for the kill command
		public float decay_time;
		public float time_remaining;
		public bool dying;

		//resets the set to default values
		public void Reset(){
			velocity = new Vector3 (0, 0, 0);
			acceleration= new Vector3 (0, 0, 0);
			drag = 0f;
			decay_time = -1;
			time_remaining = -1;
			dying = false;
		}
	}
	//Access the named set. If that set does not exist it creates it
	//If revive is true it resets the kill data
}
