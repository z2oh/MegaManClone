using UnityEngine;
using System.Collections;

public class PlayerSync : MonoBehaviour {

	public PlayerControl player;
	//I recommend you keep this value at 1
	public float rotationSpeed;
	//references
	private Transform thisTransform;
	private Transform playerPos;
	private Vector3 offset;
	private float delta;

	void Start () {
		thisTransform = GetComponent<Transform> ();
		playerPos = player.GetComponent<Transform> ();
		delta = 0;
		offset = thisTransform.position - playerPos.position;
	}
	
	// Update is called once per frame
	void Update () {
		//sync with player rotation
		thisTransform.position = playerPos.position + offset;
		//if at the right rotation stop rotation
		if (delta != 0) {
			if ((thisTransform.eulerAngles.z - player.orientation * 90) % 360 > -rotationSpeed &&
			   (thisTransform.eulerAngles.z - player.orientation * 90) % 360 < rotationSpeed) {
				delta = 0;
				thisTransform.eulerAngles = new Vector3 (thisTransform.eulerAngles.x, thisTransform.eulerAngles.y, player.orientation * 90);
			}
			//rotate the camera
			thisTransform.Rotate (new Vector3 (0, 0, delta));
		}
	}

	//start rotating
	public void Rotate(bool reverse){
		if (reverse)
			delta = -rotationSpeed;
		else
			delta = rotationSpeed;
	}
}
