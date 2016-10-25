using UnityEngine;
using System.Collections;

public class Head_Animation : MonoBehaviour
{
    private float next_blink;
    private Animator animator;
    private Animator body_animator;
    public float blink_rate;
    private GameObject parent;
    private GameObject body;
    private float head_offset_x;
    private float head_offset_y;
    private float last_blink;

    // Use this for initialization
    void Start()
    {
        next_blink = Time.time + blink_rate;
        animator = this.GetComponent<Animator>();
        parent = this.transform.parent.gameObject;
        foreach (Transform child in parent.transform)
        {
            if(child.name == "body")
            {
                print("found the body");
                body = child.gameObject;
            }
        }
        body_animator = body.GetComponent<Animator>();
        print(body_animator.GetBool("idle"));
    }

    // Update is called once per frame
    void Update()
    {
        head_offset_y = parent.transform.position.y + 0.25f;
        head_offset_x = parent.transform.position.x - 0.032f;
        //print(body_animator.GetCurrentAnimatorStateInfo(0));
        if (body_animator.GetBool("idle"))
        {
            if (body_animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 <= 0.5)
            {
                print("head wobble");
                this.transform.position = head_offset_x * Gravity.right + (head_offset_y) * Gravity.up;
            }
            else
            {
                this.transform.position = new Vector3(head_offset_x, head_offset_y, 0);
            }
        }
        else
        {
            this.transform.position = new Vector3(head_offset_x, head_offset_y, 0);
        }

        if(Time.time > next_blink)
        {
            print("Should blink");
            animator.SetBool("blink", true);
            last_blink = Time.time;
            next_blink = Time.time + blink_rate;
            print(next_blink);
        }
        else if(Time.time > last_blink + 1 && Time.time < next_blink)
        {
            print("reseting animation");
            animator.SetBool("blink", false);
        }
    }
}