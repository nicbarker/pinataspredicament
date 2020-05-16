using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
  const float skinWidth = 0.015f;
  public int horizontalRayCount = 4;
  public int verticalRayCount = 4;
  public LayerMask collisionMask;
  public float maxClimbAngle = 80;
  public float maxDescendAngle = 75;

  float horizontalRaySpacing;
  float verticalRaySpacing;

  BoxCollider2D collider;
  RaycastOrigins raycastOrigins;

  public CollisionInfo collisions;

  void Start()
  {
    collider = GetComponent<BoxCollider2D>();
    CalculateRaySpacing();
  }

  public void Move(Vector3 velocity)
  {
    collisions.Reset();
    UpdateRaycastOrigins();
    if (velocity.y < 0)
    {
      DescendSlope(ref velocity);
    }
    if (velocity.x != 0)
    {
      collisions.left = collisions.right = false;
      HorizontalCollisions(ref velocity);
    }
    if (velocity.y != 0)
    {
      collisions.above = collisions.below = false;
      VerticalCollisions(ref velocity);
    }
    if (collisions.connectedBody)
    {
      collisions.oldConnectedBodyPosition = collisions.connectedBody.transform.position;
    }

    transform.Translate(velocity);
  }

  void HorizontalCollisions(ref Vector3 velocity)
  {
    float directionX = Mathf.Sign(velocity.x);
    float rayLength = Mathf.Abs(velocity.x) + skinWidth;

    for (int i = 0; i < horizontalRayCount; i++)
    {
      Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
      rayOrigin += Vector2.up * (horizontalRaySpacing * i);
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

      if (hit)
      {
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

        if (i == 0 && slopeAngle < maxClimbAngle)
        {
          float distanceToSlopeStart = 0;
          if (slopeAngle != collisions.slopeAngleOld)
          {
            distanceToSlopeStart = hit.distance - skinWidth;
            velocity.x -= distanceToSlopeStart * directionX;
          }
          ClimbSlope(ref velocity, slopeAngle);
          velocity.x += distanceToSlopeStart * directionX;
        }

        if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
        {
          velocity.x = (hit.distance - skinWidth) * directionX;
          rayLength = hit.distance;

          if (collisions.climbingSlope)
          {
            velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
          }

          collisions.left = directionX == -1;
          collisions.right = directionX == 1;
        }
      }
    }
  }

  void VerticalCollisions(ref Vector3 velocity)
  {
    float directionY = Mathf.Sign(velocity.y);
    float rayLength = Mathf.Abs(velocity.y) + skinWidth;

    for (int i = 0; i < verticalRayCount; i++)
    {
      Vector2 rayOrigin = directionY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
      rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

      if (hit)
      {
        velocity.y = (hit.distance - skinWidth) * directionY;
        rayLength = hit.distance;

        if (collisions.climbingSlope)
        {
          velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
        }

        collisions.below = directionY == -1;
        collisions.above = directionY == 1;

        if (collisions.below)
        {
          // If this is the first impact with a moving platform and downward velocity is fast enough
          // play a small bounce animation on the moving platform
          if (collisions.connectedBody == null && velocity.y < -0.02f)
          {
            var platformBehaviour = hit.transform.gameObject.GetComponent<MovingPlatformBehaviour>();
            if (platformBehaviour != null)
            {
              platformBehaviour.playBounceAnimation();
            }
          }
          collisions.connectedBody = hit.transform;
          collisions.oldConnectedBodyPosition = hit.transform.position;
        }
      }
    }

    if (!collisions.below && collisions.connectedBody != null)
    {
      collisions.connectedBody = null;
      collisions.oldConnectedBodyPosition = new Vector2(0, 0);
    }

    if (collisions.climbingSlope)
    {
      float directionX = Mathf.Sign(velocity.x);
      rayLength = Mathf.Abs(velocity.x) + skinWidth;
      Vector2 rayOrigin = (directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

      if (hit)
      {
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
        if (slopeAngle != collisions.slopeAngle)
        {
          velocity.x = (hit.distance - skinWidth) * directionX;
          collisions.slopeAngle = slopeAngle;
        }
      }
    }
  }

  void ClimbSlope(ref Vector3 velocity, float slopeAngle)
  {
    float moveDistance = Mathf.Abs(velocity.x);
    float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
    if (velocity.y <= climbVelocityY)
    {
      velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
      velocity.y = climbVelocityY;
      collisions.below = true;
      collisions.climbingSlope = true;
      collisions.slopeAngle = slopeAngle;
    }
  }

  void DescendSlope(ref Vector3 velocity)
  {
    float directionX = Mathf.Sign(velocity.x);
    Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

    if (hit)
    {
      float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
      if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
      {
        if (Mathf.Sign(hit.normal.x) == directionX)
        {
          if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
          {
            float moveDistance = Mathf.Abs(velocity.x);
            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            velocity.y -= descendVelocityY;
          }
        }
      }
    }
  }

  void UpdateRaycastOrigins()
  {
    Bounds bounds = collider.bounds;
    bounds.Expand(skinWidth * -2);
    raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
    raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
  }

  void CalculateRaySpacing()
  {
    Bounds bounds = collider.bounds;
    bounds.Expand(skinWidth * -2);
    horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
    verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

    horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
    verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
  }

  struct RaycastOrigins
  {
    public Vector2 topLeft, topRight;
    public Vector2 bottomLeft, bottomRight;
  }

  public struct CollisionInfo
  {
    public bool above;
    public bool below;
    public bool left;
    public bool right;
    public bool climbingSlope;
    public float slopeAngle, slopeAngleOld;
    public Transform connectedBody;
    public Vector2 oldConnectedBodyPosition;

    public void Reset()
    {
      climbingSlope = false;
      slopeAngleOld = slopeAngle;
      slopeAngle = 0;
    }
  }

}