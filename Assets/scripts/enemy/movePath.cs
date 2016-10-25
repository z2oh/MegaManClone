using UnityEngine;
using System.Collections;

public class movePath : MonoBehaviour {
    public Transform endPath;
    private Vector3 front;
    private Vector3 start;
    private float secPerLap = 5f;
	// Use this for initialization
	void Start () {
        front = transform.position;
        start = endPath.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(front, start, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.time / secPerLap, 1f)));




	}







}
