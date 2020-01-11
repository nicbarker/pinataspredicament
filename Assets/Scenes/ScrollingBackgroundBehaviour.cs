using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgroundBehaviour : MonoBehaviour
{
    public GameObject player;
    public float fractionOfPlayerPosition = 1f;

    private float initialXPosition;
    private GameObject secondCopy;
    private float copyInitialXPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialXPosition = transform.position.x;
        secondCopy = new GameObject();
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
            transform.position.y,
            transform.position.z
        );
        secondCopy.transform.position = new Vector3(
            Mathf.Max(copyInitialXPosition + player.transform.position.x * fractionOfPlayerPosition, copyInitialXPosition),
            transform.position.y,
            transform.position.z
        );

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        float minOffset = Mathf.Max(sprite.bounds.size.x, 19.36338f);
        if (initialXPosition + player.transform.position.x * fractionOfPlayerPosition < player.transform.position.x - minOffset)
        {
            initialXPosition += minOffset * 2;
        }

        if (copyInitialXPosition + player.transform.position.x * fractionOfPlayerPosition < player.transform.position.x - minOffset)
        {
            copyInitialXPosition += minOffset * 2;
        }
    }
}
