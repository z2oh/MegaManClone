using UnityEngine;
using System.Collections;

public class JumpTest : MonoBehaviour {

	// Test Mode settings
	// if ballistic up is true object follows a ballistic trajectory upwards
	// otherwise it follows a linear trajectory
	//!Warning If ballistic up is true and ballistic down id false the code will not function properly
	public bool ballisticUp;
	// as with ballistic up but determines downward trajectory
	public bool ballisticDown;

	//horizontal movement speed
	public float horizontalVelocity;

	//determines how far down the code should look for the floor
	public float testLength;
	public float testWidth;

	//Ballistic Settings
	//launch velocity
	public float intialVelocity;

	//fixed settings
	//upward velocity
	public float jumpVelocity;
	//downward velocity
	public float fallVelocity;
	//if using a fixed upward velocity and a ballistic downward velocity this will control how quickly it changes from a fixed trajectory to a ballistic one 
	public float peakVelocityOffset;
	//number of updates the jump will last
	public int jumpDuration;
	//internal timer
	private int jumpTime;

	//true if the player is standing on the ground
	private bool grounded;
	private Rigidbody2D body;

	void Start () {
		//gets a referance to the player
		body = GetComponent<Rigidbody2D> ();
		//turns off gravity if using a fixe fall speed
		if (!ballisticDown)
			body.gravityScale = 0;
		jumpTime = -1;
	}

	void FixedUpdate () {
		//test to see if player is on the ground
		grounded = testDown ();
		//horizonatal directional input, does not effect vertical velocity
		body.velocity = new Vector2 (horizontalVelocity * Input.GetAxis("Horizontal"), body.velocity.y);
		//I segregated the code for readability
		if (ballisticUp)
			balUp ();
		else
			fixedUp ();
	}

	void balUp(){
		//pretty strait forward
		if (grounded && Input.GetButton ("Jump"))
			body.velocity = body.velocity + new Vector2 (0, intialVelocity);
	}

	void fixedUp(){
		//if one the ground and jumping
		if (grounded && Input.GetButton ("Jump")) {
			//set the timer and start moving 
			jumpTime = jumpDuration;
			body.velocity = body.velocity + new Vector2 (0, jumpVelocity);
		}
		//if the object is suppossed to keep moving upward
		if (jumpTime > 0) {
			//test to make sure nothing stopped the object
			//i.e. the player didn't hit a ceiling
			if (body.velocity.y > 0) {
				//turn off gravity while moving upward
				if (ballisticDown)
					body.AddForce (-Physics2D.gravity * body.mass);
				//maintain constant velocity
				if (body.velocity.y < jumpVelocity)
					body.velocity = new Vector2 (body.velocity.x, jumpVelocity);
				//decrement the time
				jumpTime--;
			} else {
				//we hit the ceiling start falling
				jumpTime = -1;
			}
		//if the player has reached the jump apex
		} else if (jumpTime == 0) {
			//reset upward velocity to the offset velocity 
			body.velocity = new Vector2 (body.velocity.x, peakVelocityOffset);
			//start falling
			jumpTime = -1;
		} else {
			//object is falling
			// if using gravity let unity take care of it, else use the fixed fall method
			if (!ballisticDown)
				fixedDown ();
		}
	}

	void fixedDown(){
		//fall if not on the ground
		if (!grounded)
			body.velocity = new Vector2 (body.velocity.x, -fallVelocity);
	}

	bool testDown(){
		//only look at the ground layer
		int groundLayer = LayerMask.GetMask ("Ground");
		//draws a line under the player if that line itersects the ground return true, else return false
		return (bool)Physics2D.Linecast (new Vector2 (-testWidth/2, -testLength) + body.position,
			new Vector2 (testWidth/2, -testLength) + body.position, groundLayer);
	}
}
