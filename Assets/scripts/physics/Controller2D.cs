using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collision_mask;

    const float skin_width = 0.15f;

    public int horizontal_ray_count = 10;
    public int vertical_ray_count = 10;

    float max_climb_angle = 80;

    float horizontal_ray_spacing;
    float vertical_ray_spacing;

    BoxCollider2D collider;
    RaycastOrigins raycast_origins;
    public CollisionInfo collisions;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if(velocity.x != 0)
        {
            HorizontalCollisisons(ref velocity);
        }
        if(velocity.y != 0)
        {
            VerticalCollisisons(ref velocity);
        }
        transform.Translate(velocity);
    }

    void HorizontalCollisisons(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skin_width;

        for(int i = 0; i < vertical_ray_count; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycast_origins.bottomLeft : raycast_origins.bottomRight;
            rayOrigin += Vector2.up * (horizontal_ray_spacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collision_mask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(i == 0 & slopeAngle <= max_climb_angle)
                {
                    ClimbSlope(ref velocity, slopeAngle);
                }

                velocity.x = (hit.distance - skin_width) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
                if(collisions.left)
                {
                    collisions.leftColl = hit.collider;
                }
                if(collisions.right)
                {
                    collisions.rightColl = hit.collider;
                }
            }
        }
    }

    void VerticalCollisisons(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skin_width;

        for(int i = 0; i < vertical_ray_count; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycast_origins.bottomLeft : raycast_origins.topLeft;
            rayOrigin += Vector2.right * (vertical_ray_spacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collision_mask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if(hit)
            {
                velocity.y = (hit.distance - skin_width) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
                if(collisions.above)
                {
                    collisions.aboveColl = hit.collider;
                }
                if(collisions.below)
                {
                    collisions.belowColl = hit.collider;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {

    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skin_width * -2);

        raycast_origins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycast_origins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycast_origins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycast_origins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skin_width * -2);

        horizontal_ray_count = Mathf.Clamp(horizontal_ray_count, 2, int.MaxValue);
        vertical_ray_count = Mathf.Clamp(vertical_ray_count, 2, int.MaxValue);

        horizontal_ray_spacing = bounds.size.y / (horizontal_ray_count - 1);
        vertical_ray_spacing = bounds.size.x / (vertical_ray_count - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public Collider2D aboveColl, belowColl;
        public Collider2D leftColl, rightColl;

        public void Reset()
        {
            above = below = left = right = false;
            aboveColl = belowColl = leftColl = rightColl = null;
        }
    }
}