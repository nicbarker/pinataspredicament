using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgroundBehaviour : MonoBehaviour
{
  public GameObject player;
  public float fractionOfPlayerPosition = 1f;
  public float YfractionOfPlayerPosition = 1f;

  private GameObject secondCopy;

  private float initialXPosition;
  private float copyInitialXPosition;

  private float initialYPosition;
  // Start is called before the first frame update
  void Start()
  {
    initialXPosition = transform.position.x;
    initialYPosition = transform.position.y;
    secondCopy = new GameObject(this.name + "ScrollingBackgroundClone");
    secondCopy.transform.parent = this.transform.parent;
    secondCopy.AddComponent<SpriteRenderer>();
    secondCopy.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
    secondCopy.transform.localScale = transform.localScale;
    copyInitialXPosition = transform.position.x + Mathf.Max(secondCopy.GetComponent<SpriteRenderer>().bounds.size.x, 19.36338f);
    secondCopy.transform.position = new Vector3(
        copyInitialXPosition,
        transform.position.y,
        transform.position.z
    );
  }

  // Update is called once per frame
  void LateUpdate()
  {
    transform.position = new Vector3(
        Mathf.Max(initialXPosition + player.transform.position.x * fractionOfPlayerPosition, initialXPosition),
        Mathf.Max(initialYPosition + player.transform.position.y * YfractionOfPlayerPosition, initialYPosition),
        transform.position.z
    );
    secondCopy.transform.position = new Vector3(
        Mathf.Max(copyInitialXPosition + player.transform.position.x * fractionOfPlayerPosition, copyInitialXPosition),
        Mathf.Max(initialYPosition + player.transform.position.y * YfractionOfPlayerPosition, initialYPosition),
        transform.position.z
    );

    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
    float objectWidth = Mathf.Max(sprite.bounds.size.x, 19.36338f);
    if (transform.position.x < player.transform.position.x - objectWidth)
    {
      initialXPosition += objectWidth * 2;
    }
    else if (transform.position.x > player.transform.position.x + objectWidth)
    {
      initialXPosition -= objectWidth * 2;
    }

    if (secondCopy.transform.position.x < player.transform.position.x - objectWidth)
    {
      copyInitialXPosition += objectWidth * 2;
    }
    else if (secondCopy.transform.position.x > player.transform.position.x + objectWidth)
    {
      copyInitialXPosition -= objectWidth * 2;
    }
  }
}
