using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
  public float speed = 1f;
  public bool movingRight = true;

  public bool initiallyMoving = true;
  // Update is called once per frame
  void Update()
  {
    if (initiallyMoving)
    {
      var change = Time.deltaTime * speed;
      transform.position = new Vector3(transform.position.x + (change * (movingRight ? 1 : -1)), transform.position.y, transform.position.z);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Debug.Log("trigger");
    switch ((Layers)collision.gameObject.layer)
    {
      case Layers.EdgeCollider:
        movingRight = !movingRight;
        return;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    initiallyMoving = true;
  }
}
