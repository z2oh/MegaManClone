using UnityEngine;
using System.Collections;

public class Body_Animation : MonoBehaviour {
    private Animator animator;
    private GameObject parent;

    // Use this for initialization
    void Start () {
        animator = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("left") || Input.GetKey("right"))
        {
            setBools(false, false, true);
        }
        else if(Input.GetKey("space") || Input.GetKey("up") || Input.GetKey("w"))
        {
            setBools(false, true, false);
        }
        else
        {
            setBools(true, false, false);
        }
	}

    void setBools(bool idle, bool jump, bool run)
    {
        animator.SetBool("idle", idle);
        animator.SetBool("jump", jump);
        animator.SetBool("run", run);
    }
}
