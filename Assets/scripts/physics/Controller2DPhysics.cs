using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2D))]
public class Controller2DPhysics : MonoBehaviour {

	// The current velocity of the object
	//changing this value will NOT change the velocity of the object
	//Use the set velocity method instead
	[HideInInspector]
	public Vector2 velocity;

	//A hash map of all of the different physics sets
	Dictionary<string, physics_data> physics;

	//The Controller2D that this script uses
	Controller2D controller;

	//These values are used by the lock velocity method
	//min is the minimum allowed velocity and max is the maximum allowed velocity
	//lock duration determines how long the system is locked
	Vector2 min, max;
	float lock_duration;

	void Start () {
		lock_duration = 0;

		controller = GetComponent<Controller2D> ();
		physics = new Dictionary<string, physics_data> ();
		//Create the default physics set
		physics ["DEFAULT"] = new physics_data ();
		physics ["DEFAULT"].Reset ();
	}

	// This method needs to be a fixed update
	// There really isn't anyway around that
	void FixedUpdate () {
		//the change in time
		//as this is a fixed update this value is a constant
		float delta = Time.deltaTime;

		//calculate velocity from the physics sets
		velocity = new Vector2 (0, 0);
		//itereate through the hash map
		List<string> keys = new List<string> (physics.Keys);
		foreach (string key in keys) {
			physics_data phys = physics [key];
			//the kill method allows you to tell a physics set to return to initial values
			//this reduces the remaining time stored in the physics_data struct
			if (phys.dying)
				phys.time_remaining -= delta;
			//if the physics data set is dead it needs to be returened to default values
			//as it will likely be used again it is kept in the dictionary
			if (phys.dying && phys.time_remaining <= 0)//Physics set is dead
				phys.Reset ();
			else{//Physics set is alive
				//accelerate
				phys.velocity += delta * phys.acceleration - phys.velocity * phys.drag;
				//reset the acceleration vector
				phys.acceleration = new Vector2 (0, 0);

				//enforce the physics locks
				//this currently has a major logical bug in it
				if (lock_duration > 0) {
					if (phys.velocity.x < min.x)
						phys.velocity = new Vector2 (min.x, phys.velocity.y);
					if (phys.velocity.x > max.x)
						phys.velocity = new Vector2 (max.x, phys.velocity.y);
					if (phys.velocity.y < min.y)
						phys.velocity = new Vector2 (phys.velocity.x, min.y);
					if (phys.velocity.y > max.y)
						phys.velocity = new Vector2 (phys.velocity.x, max.y);
				}
				//enforce collisions
				if (controller.collisions.left && phys.velocity.x < 0)
					phys.velocity = new Vector2 (0, phys.velocity.y);
				if(controller.collisions.right && phys.velocity.x > 0)
					phys.velocity = new Vector2 (0, phys.velocity.y);
				if (controller.collisions.below && phys.velocity.y < 0)
					phys.velocity = new Vector2 (phys.velocity.x, 0);
				if(controller.collisions.above && phys.velocity.y > 0)
					phys.velocity = new Vector2 (phys.velocity.x, 0);
				//update the net velocity
				//if the physics set is dying linearly decay to zero velocity
				velocity += phys.velocity * (phys.dying ? (phys.time_remaining / phys.decay_time) : 1);
			}
			//save the changes to the physics sets
			physics [key] = phys;
		}
		//decreament the lock time
		if(lock_duration > 0)
			lock_duration -= delta;
		//move the object
		controller.Move (delta * new Vector3(velocity.x, velocity.y, 0));
	}
	//This method lock the movement of the objcet to a specific range
	//this will override any  existing physics set but not the boundries
	//DO NOT use this method yet. it is a work in progress
	public void Force_Velocity(Vector2 v1, Vector2 v2, float time){
		min = new Vector2 ((v1.x > v2.x) ? v2.x : v1.x, (v1.y > v2.y) ? v2.y : v1.y);
		max = new Vector2 ((v1.x > v2.x) ? v1.x : v2.x, (v1.y > v2.y) ? v1.y : v2.y);
		lock_duration = time;
	}

	//Sets the porperties of a physics set. By default modifies the DEFAULT set
	//by default awakens the set accesssed
	public void Set_Velocity (Vector2 value, string name = "DEFAULT", bool revive = true){
		physics_data phys = Access (name, revive);
		phys.velocity = value;
		physics [name] = phys;
	}
	public void Accelerate (Vector2 value, string name = "DEFAULT", bool revive = true){
		physics_data phys = Access (name, revive);
		phys.acceleration += value;
		physics [name] = phys;
	}
	public void Set_Drag (float value, string name = "DEFAULT", bool revive = true){
		physics_data phys = Access (name, revive); 
		phys.drag = value;
	}
	//Kills the named physics set.
	//Default cannot be killed
	//If the set is already dying when this is called the 
	public void Kill (float time, string name){
		//default cannot be killed
		if (name == "DEFAULT" || !physics.ContainsKey (name))
			return;
		//get the relevent set
		physics_data phys = Access(name, false);
		//if already dying
		if (phys.dying) {
			//use the shorter time remaining
			if (time < phys.time_remaining)
				phys.time_remaining = time;
		} else {
			//otherwise start dying
			phys.decay_time = time;
			phys.time_remaining = time;
		}
		//indicate that the set is dying
		phys.dying = true;
		//save the changes
		physics [name] = phys;
	}

	//returns the named physics set
	//if the name is not found creates a new set
	public physics_data Get_Physics_Data(string name = "DEFAULT"){
		if(!physics.ContainsKey(name)){
			physics [name] = new physics_data ();
			physics [name].Reset ();
		}
		return physics [name];
	}

	//The physics data struct
	public struct physics_data
	{
		//the velocity of the set
		public Vector2 velocity;
		//acceleration of the set
		public Vector2 acceleration;
		//linear drag
		public float drag;
		//date used for the kill command
		public float decay_time;
		public float time_remaining;
		public bool dying;

		//resets the set to default values
		public void Reset(){
			velocity = new Vector2 (0, 0);
			acceleration= new Vector2 (0, 0);
			drag = 0f;
			decay_time = -1;
			time_remaining = -1;
			dying = false;
		}
	}
	//Access the named set. If that set does not exist it creates it
	//If revive is true it resets the kill data
	physics_data Access (string name, bool revive){
		physics_data phys;
		//check to set if set exists
		if (!physics.ContainsKey (name)) {
			//if it doesn't creates it
			phys = new physics_data ();
			phys.Reset ();
		} else //otherwise finds the set
			phys = physics [name];
		//revives the set
		if(revive){
			phys.dying = false;
			phys.decay_time = -1;
			phys.time_remaining = -1;
		}
		return phys;
	}
}
