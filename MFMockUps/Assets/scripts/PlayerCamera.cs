using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	//player to follow
	public Transform player;

	//camera location
	private Transform location;
	private Vector3 offset;

	void Start () {
		location = GetComponent<Transform> ();
		offset = location.position - player.position;
	}
	

	void Update () {
		location.position = player.position + offset;
	}
}
