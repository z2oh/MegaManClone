using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public Transform player;

	Transform camera;
	Vector3 offset;
	// Use this for initialization
	void Start () {
		camera = GetComponent<Transform> ();
		offset = camera.position - player.position;
	}
	
	// Update is called once per frame
	void Update () {
		camera.position = offset + player.position;
	}
}
