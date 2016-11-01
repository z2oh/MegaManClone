using UnityEngine;
using System.Collections;

public class Head_Animation : MonoBehaviour
{
	// The next time that a blink can occur.
    private float next_blink;
	// The rate at which the player blinks.
    public float blink_rate;
	// The SpriteRenderer for the head.
	private SpriteRenderer head;
	// The animator for the head.
    private Animator animator;
	// The animator for the body.
    private Animator body_animator;

	private GameObject player;
    private GameObject body;
	// The offset for the head.
    private float head_offset_x;
    private float head_offset_y;
    private float head_offset_flipx;
    private float last_blink;

    // Use this for initialization
    void Start()
    {
		head = GetComponent<SpriteRenderer>();
        next_blink = Time.time + blink_rate;
        animator = GetComponent<Animator>();
		player = transform.parent.gameObject;
        foreach (Transform child in transform.parent)
        {
            if(child.name == "body")
            {
                body = child.gameObject;
            }
        }
        body_animator = body.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if(!player.GetComponent<Movement>().IsFacingLeft())
		{
			head.flipX = true;
		}
		else
		{
			head.flipX = false;
		}
        //head_offset_y = 0.035f;
        //head_offset_x = -0.04f;
        //head_offset_flipx = 0.04f;

        if(Time.time > next_blink)
        {
            animator.SetBool("blink", true);
            last_blink = Time.time;
            next_blink = Time.time + blink_rate;
        }
        else if(Time.time > last_blink + 1 && Time.time < next_blink)
        {
            animator.SetBool("blink", false);
        }
    }
}