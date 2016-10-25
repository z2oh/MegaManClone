using UnityEngine;
using System.Collections;

public class LockTest : MonoBehaviour {

	public Vector3 max, min;
	public float time;

	bool once;
	// Use this for initialization
	void Start () {
		once = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (once) {
			GetComponent<Controller2DPhysics> ().Lock_Velocity (max, min, time);
			once = false;
		}
	}
}
