using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2D))]
public class Controller2DPhysics : MonoBehaviour {

	// Use this for initialization
	[HideInInspector]
	public Vector2 velocity;

	Dictionary<string, physics_data> physics;

	Controller2D controller;
	Vector2 min, max;
	float lock_duration;

	// Use this for initialization
	void Start () {
		lock_duration = 0;

		controller = GetComponent<Controller2D> ();
		physics = new Dictionary<string, physics_data> ();
		physics ["DEFAULT"] = new physics_data ();
		physics ["DEFAULT"].Reset ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		float delta = Time.deltaTime;

		//calculate net values
		velocity = new Vector2 (0, 0);
		Vector2 net_acceleration = new Vector2 (0, 0);
		List<string> keys = new List<string> (physics.Keys);
		foreach (string key in keys) {
			physics_data phys = physics [key];
			if (phys.dying == true)
				phys.time_remaining -= delta;
			if (phys.dying == true && phys.time_remaining < 0)
				physics.Remove (key);
			else {
				phys.velocity += delta * phys.acceleration - phys.velocity * phys.damping;
				phys.acceleration = new Vector2 (0, 0);
				if (lock_duration > 0) {
					if (phys.velocity.x < min.x)
						phys.velocity = new Vector2 (min.x, phys.velocity.y);
					if (phys.velocity.x > max.x)
						phys.velocity = new Vector2 (max.x, phys.velocity.y);
					if (phys.velocity.y < min.y)
						phys.velocity = new Vector2 (phys.velocity.x, min.y);
					if (phys.velocity.y > max.y)
						phys.velocity = new Vector2 (phys.velocity.x, max.y);

					if (controller.collisions.left && phys.velocity.x < 0)
						phys.velocity = new Vector2 (0, phys.velocity.y);
					if(controller.collisions.right && phys.velocity.x > 0)
						phys.velocity = new Vector2 (0, phys.velocity.y);
					if (controller.collisions.below && phys.velocity.y < 0)
						phys.velocity = new Vector2 (phys.velocity.x, 0);
					if(controller.collisions.above && phys.velocity.y > 0)
						phys.velocity = new Vector2 (phys.velocity.x, 0);
				}

				physics [key] = phys;
				velocity += phys.velocity;
			}
		}
		if(lock_duration > 0)
			lock_duration -= delta;

		controller.Move (delta * new Vector3(velocity.x, velocity.y, 0));
	}

	public void Force_Velocity(Vector2 v1, Vector2 v2, float time){
		min = new Vector2 ((v1.x > v2.x) ? v2.x : v1.x, (v1.y > v2.y) ? v2.y : v1.y);
		max = new Vector2 ((v1.x > v2.x) ? v1.x : v2.x, (v1.y > v2.y) ? v1.y : v2.y);
		lock_duration = time;
	}

	public void Set_Velocity (Vector2 value, string name = "DEFAULT"){
		if (!physics.ContainsKey (name)) {
			physics [name] = new physics_data ();
			physics [name].Reset ();
		}
		physics_data phys = physics [name];
		phys.velocity = value;
		physics [name] = phys;
	}

	public void Accelerate (Vector2 value, string name = "DEFAULT"){
		if (!physics.ContainsKey (name)) {
			physics [name] = new physics_data ();
			physics [name].Reset ();
		}
		physics_data phys = physics [name];
		phys.acceleration += value;
		physics [name] = phys;
	}

	public void Set_Damping (float value, string name = "DEFAULT"){
		if (!physics.ContainsKey (name)) {
			physics [name] = new physics_data ();
			physics [name].Reset ();
		}
		physics_data phys = physics [name]; 
		phys.damping = value;
	}

	public void Kill (float time, string name){
		if (name == "DEFAULT" || !physics.ContainsKey (name))
			return;
		physics_data phys = physics [name];
		phys.decay_time = time;
		phys.time_remaining = time;
		phys.dying = true;
		physics [name] = phys;
	}

	public physics_data Get_Physics_Data(string name = "DEFAULT"){
		if(!physics.ContainsKey(name)){
			physics [name] = new physics_data ();
			physics [name].Reset ();
		}
		return physics [name];
	}
	public struct physics_data
	{
		public Vector2 velocity;
		public Vector2 acceleration;
		public float damping;
		public float decay_time;
		public float time_remaining;
		public bool dying;

		public void Reset(){
			velocity = new Vector2 (0, 0);
			acceleration= new Vector2 (0, 0);
			damping = 0f;
			decay_time = -1;
			time_remaining = -1;
			dying = false;
		}
	}
}
