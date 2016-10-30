using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	public Transform player;

	Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position - player.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = offset + player.position;
	}
}
