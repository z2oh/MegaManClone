﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Movement : MonoBehaviour
{

    // jumpHeight and timeToJumpApex are used to determine the correct gravity and jump forces.
    public float jump_height = 4;
    public float time_to_jump_apex = .4f;

    // accelerationTimeAirborne and accelerationTimeGrounded have to do with the 'smoothing' factor when changing directions. It should be easier to change directions when on the ground.
    float acceleration_time_airborne = .1f;
    float acceleration_time_grounded = .05f;

    // moveSpeed is the speed of the player.
    float move_speed = 6;

    // These values are used to update position and velocity every frame.
    float jump_velocity;
    float gravity;
    Vector3 velocity;
    float velocity_x_smoothing;

    // The controller that is used in collision detection.
    Controller2D controller;

    // The sprite of the player.
    SpriteRenderer sprite;

    void Start()
    {
        // We set the controller and input handler for this object, as well as calculating the gravity and jump velocity.
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jump_height) / Mathf.Pow(time_to_jump_apex, 2);
        jump_velocity = Mathf.Abs(gravity) * time_to_jump_apex;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // If the player is actively colliding with something above or below it, the velocity in the y direction is reset to 0.
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        // We make the input vector and set it to zero.
        Vector2 input = Vector2.zero;

        // We grab the input values from the active controller.
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(input.x > 0)
        {
            sprite.flipX = true;
        }
        else if(input.x < 0)
        {
            sprite.flipX = false;
        }
        // If space is pressed and the player is touching the ground, we set the y velocity to be jump velocity.
        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jump_velocity;
        }

        // We find the target velocity for x.
        float targetVelocityX = input.x * move_speed;
        // We move the velocity in the x direction closer to the target x velocity. This makes changing directions much smoother. We use different acceleration values for when the player is airborne or on the ground.
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocity_x_smoothing, (controller.collisions.below) ? acceleration_time_grounded : acceleration_time_airborne);
        // We apply gravity to the player.
        velocity.y += gravity * Time.deltaTime;
        // We send the calculated velocity vector to the controller which will actually move the object.
        controller.Move(velocity * Time.deltaTime);
    }
}