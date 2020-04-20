using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION
{
  LEFT,
  RIGHT
}

public class MovingPlatformBehaviour : MonoBehaviour
{
  public float speed = 1f;
  public DIRECTION direction = DIRECTION.RIGHT;

  public bool moving = true;
  public bool activateOnTouch = true;
  // Update is called once per frame
  private float bounceDelay = 0;
  private float bounceTimer = 0;
  private float preBounceY = 0;

  void Start()
  {
    if (activateOnTouch)
    {
      moving = false;
    }
  }

  void Update()
  {
    if (moving)
    {
      var change = Time.deltaTime * speed;
      transform.position = new Vector3(transform.position.x + (change * (direction == DIRECTION.RIGHT ? 1 : -1)), transform.position.y, transform.position.z);
    }

    if (bounceTimer > 0 && bounceDelay <= 0)
    {
      bounceTimer -= Time.deltaTime * 15;
      float newYPosition = (1 - Mathf.Pow((bounceTimer - 0.5f), 2) * 4) * 0.10f;
      transform.position = new Vector3(
        transform.position.x,
        preBounceY - newYPosition,
        transform.position.z
      );
    }
    else if (bounceDelay > 0)
    {
      bounceDelay -= Time.deltaTime * 15;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Debug.Log("trigger");
    switch ((Layers)collision.gameObject.layer)
    {
      case Layers.EdgeCollider:
        direction = direction == DIRECTION.RIGHT ? DIRECTION.LEFT : DIRECTION.RIGHT;
        return;
    }
  }

  public void playBounceAnimation()
  {
    preBounceY = transform.position.y;
    bounceDelay = 0.08f;
    bounceTimer = 1.0f;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (activateOnTouch)
    {
      moving = true;
    }

    if (bounceTimer <= 0 && collision.relativeVelocity.magnitude > 3 && (collision.gameObject.transform.position - transform.position).normalized.y > 0.1)
    {
      playBounceAnimation();
    }
  }
}
