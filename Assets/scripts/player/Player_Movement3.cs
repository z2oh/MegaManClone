using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Controller2DPhysics))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Gravity))]
public class Player_Movement3 : MonoBehaviour {

    public float launch_speed, terminal_speed, gravity_magnatude, horz_movement_speed, impulse_time;
    float jump_time;
    Vector3 velocity;
    //Controller2DPhysics physics;
    Controller2D controller;

    SpriteRenderer sprite;

    bool facing_right;

    void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        //physics = GetComponent<Controller2DPhysics> ();
        controller = GetComponent<Controller2D>();
    }

    void Start() {
        facing_right = true;
        velocity = new Vector3(0, 0, 0);
    }

    //this should really be a fixed update but it might be ok if it is an update instead
    void FixedUpdate() {
        //calcualte the change in time
        float delta = Time.deltaTime;
        //update the player velocity
        velocity = (Calculate_Horz_Velocity(delta) + Calculate_Vert_Velocity(delta));
        controller.Move(velocity * delta);

    }

    Vector3 Calculate_Vert_Velocity(float time) {
        float base_speed = Vector3.Dot(Gravity.up, velocity);
        if (Test_Direction(-1 * Gravity.up)) {
            jump_time = impulse_time;
            if (Input.GetButton("Jump")) {
                base_speed = launch_speed;
            }
            else {//don't jump if they didn't press jump
                base_speed = 0;
            }
        }
        else {//if in the air
            if (base_speed > 0) {
                jump_time -= time;
                if (!Input.GetButton("Jump"))
                    jump_time = 0;
                if (Test_Direction(Gravity.up)) {
                    jump_time = 0;
                    base_speed = 0;
                }
            }
            else
                jump_time = 0;
            //if the player releases jump cut the jump short
            if (jump_time > 0) {
                base_speed = launch_speed;
            }
            else {
                //accelerate downward
                base_speed -= time * gravity_magnatude;
                if (base_speed < -terminal_speed)
                    base_speed = -terminal_speed;
            }
        }
        //return the new data
        return Gravity.up * base_speed;
    }

    Vector3 Calculate_Horz_Velocity(float time) {
        Vector3 input = Input.GetAxisRaw("Horizontal") * Gravity.right;
        if (input.x > 0) {
            foreach (Transform child in transform) {
                child.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else if (input.x < 0) {
            foreach (Transform child in transform) {
                child.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        sprite.flipX = facing_right;
        float speed = horz_movement_speed;
        return input * speed;
    }

    public bool Test_Direction(Vector3 dir) {
        if (dir.y > 0)
            return controller.collisions.above;
        if (dir.y < 0)
            return controller.collisions.below;
        if (dir.x > 0)
            return controller.collisions.right;
        if (dir.x < 0)
            return controller.collisions.left;
        return false;
    }

    public Vector3 Get_Forward() {
        if (facing_right)
            return Gravity.right;
        return Gravity.right * -1;
    }
    public Vector3 Get_Up() {
        return Gravity.up;
    }
}
