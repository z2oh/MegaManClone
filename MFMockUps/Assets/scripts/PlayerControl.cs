using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	//records if the player is touching the ground
	[HideInInspector]
	public bool grounded;
	//records the players orientation
	//0 is upright
	//1 is rotated counter clockwise 90 degress
	//ect.
	[HideInInspector]
	public int orientation;

	//force applied for horizontal movement on the ground
	public float groundSpeed;
	//force applied for horizontal movement in the air
	public float airSpeed;
	//determines how far down the script looks for the ground
	public float checkDist;
	//jump acceleration this combined with checkDist determine jump height
	public float jumpForce;
	//determines the length of the ground test linecast
	//its for finetuning ledge behaviour
	public float groundCheckWidth;
	//this script uses its own gravity vector and ignores the Physics2D attribute
	public float gravityStrength;
	//lets the script tell the camera to rotate
	public PlayerSync playerCamera;

	//orientation vectors
	private Rigidbody2D rb;
	private Vector2 gravity;
	private Vector2 jump;
	private Vector2 leftCheck;
	private Vector2 rightCheck;
	private Vector2 downCheck1;
	private Vector2 downCheck2;
	private Vector2 groundMove;
	private Vector2 airMove;

	void Start () {
		//get a referance to the object's physics
		rb = GetComponent<Rigidbody2D> ();
		//set all of the orientation vectors of upright orientation vectors
		orientation = 0;
		gravity = new Vector2 (0.0f, gravityStrength);
		jump = new Vector2 (0, jumpForce);
		leftCheck = new Vector2 (-checkDist, 0);
		rightCheck = new Vector2 (checkDist, 0);
		downCheck1 = new Vector2 (-groundCheckWidth / 2, -checkDist);
		downCheck2 = new Vector2 (groundCheckWidth/2, -checkDist);
		groundMove = new Vector2 (groundSpeed, 0);
		airMove = new Vector2 (airSpeed, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//acceleration downwards regardless of what downwards is
		rb.AddForce (gravity);
		//test to see if on ground
		TestDown ();

		//if on the ground
		if (grounded) {
			//if jumping
			if (Input.GetButton ("Jump"))
				rb.velocity = rb.velocity + jump;
			//horizontal acceleration
			rb.AddForce (Input.GetAxis ("Horizontal") * groundMove);
		// not on ground
		} else {
			//test to see if player jumped onto gravity pad
			if (Input.GetAxis ("Horizontal") < 0 && TestLeft ())
				RotateAll (true);
			if (Input.GetAxis ("Horizontal") > 0 && TestRight ())
				RotateAll (false);
			//air movement
			rb.AddForce (Input.GetAxis ("Horizontal")* airMove);
		}
	}
	//returns the input rotated by 90 degress
	//rotates counter clockwise if not reversed
	//clockwise if reversed
	private Vector2 Rotate (Vector2 input, bool reverse){
		if (reverse)
			return new Vector2 (input.y, -input.x);
		return new Vector2 (-input.y, input.x);
	}
	//rotates all of the orientation vectors
	//sets the orientation attribute
	//tells camera to rotate
	private void RotateAll (bool reverse){
		if (reverse)
			orientation = (orientation - 1) % 4;
		else
			orientation = (orientation + 1) % 4;
		gravity = Rotate(gravity, reverse);
		jump = Rotate (jump, reverse);
		leftCheck = Rotate (leftCheck, reverse);
		rightCheck = Rotate (rightCheck, reverse);
		downCheck1 = Rotate (downCheck1, reverse);
		downCheck2 = Rotate (downCheck2, reverse);
		groundMove = Rotate (groundMove, reverse);
		airMove = Rotate (airMove, reverse);

		playerCamera.Rotate (reverse);
	}
	//tests for walls and floors in various directions
	private void TestDown(){
		Vector2 start = rb.position + downCheck1;
		Vector2 finish = rb.position + downCheck2;
		int groundLayer = LayerMask.GetMask ("Ground", "Wall");
		grounded = Physics2D.Linecast (start, finish, groundLayer);
	}
	private bool TestLeft(){
		Vector2 start = rb.position + leftCheck;
		Vector2 finish = rb.position;
		int groundLayer = LayerMask.GetMask ("Ground");
		return (bool)(Physics2D.Linecast (start, finish, groundLayer));
	}
	private bool TestRight(){
		Vector2 start = rb.position + rightCheck;
		Vector2 finish = rb.position;
		int groundLayer = LayerMask.GetMask ("Ground");
		return (bool)(Physics2D.Linecast (start, finish, groundLayer));
	}
}