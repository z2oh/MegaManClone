using UnityEngine;
using System.Collections;

public class Arms_Animation : MonoBehaviour {
    private Animator animator;
	private GameObject player;
	private SpriteRenderer arms;

	// Use this for initialization
	void Start () {
	    animator = GetComponent<Animator>();
		player = transform.parent.gameObject;
		arms = GetComponent<SpriteRenderer>();
	}

    void Update()
    {
        if (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("left") || Input.GetKey("right"))
        {
            setBools(false, false, true);
        }
        else if (Input.GetKey("space") || Input.GetKey("up") || Input.GetKey("w"))
        {
            setBools(false, true, false);
        }
        else
        {
            setBools(true, false, false);
        }
		if(!player.GetComponent<Movement>().IsFacingLeft())
		{
			arms.flipX = true;
		}
		else
		{
			arms.flipX = false;
		}
	}

    void setBools(bool idle, bool jump, bool run)
    {
        animator.SetBool("idle", idle);
        animator.SetBool("jump", jump);
        animator.SetBool("run", run);
    }
}

