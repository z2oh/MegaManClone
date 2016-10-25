using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour
{

    public Vector2 speed = new Vector2(10, 10);
    public Vector2 direction = new Vector2(-1, 0);
    private Vector2 movement;
     Controller2DPhysics physics;
    private Rigidbody2D rigidbodyComponent;
    // Use this for initialization
    void Awake()
    {
        physics = GetComponent<Controller2DPhysics>();
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
        physics.Set_Velocity(velocity);
    }
}


