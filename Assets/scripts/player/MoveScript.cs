using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour
{

    public Vector2 speed = new Vector2(10, 10);
    public Vector2 direction = new Vector2(-1, 0);
    private Vector3 movement;
    Player_Movement move;
     Controller2DPhysics physics;
    private Rigidbody2D rigidbodyComponent;
    // Use this for initialization
    void Awake()
    {
        physics = GetComponent<Controller2DPhysics>();//get the physics from 2d physics
        move = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
   /* void FixedUpdate()
    {
        if (rigidbodyComponent != null)
        {
            rigidbodyComponent.velocity = movement;

        }
    }*/
public void Fire(Vector2 velocity)
    {
        physics.Set_Velocity(velocity); //set the constant velocity for the bullet
        movement = move.Get_Forward(); //set direction??
    }
//public void Direction(Vector3 Get_Forward)
   // {
        
       // Vector3 direction = Get_Forward(); // Get the forward direction and shoot in that direction
   // }



}


