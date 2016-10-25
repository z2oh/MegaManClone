using UnityEngine;
using System.Collections;
using System;

public class Player_Animation : MonoBehaviour {
    private float last_position_x;
	// Use this for initialization
	void Start () {
        last_position_x = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        last_position_x = Math.Abs(last_position_x);
            foreach (Transform child in this.transform)
        {
          
        }
    }
}
