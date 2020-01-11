using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgroundBehaviour : MonoBehaviour
{
    public GameObject player;
    public float fractionOfPlayerPosition = 1f;

    private float initialXPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialXPosition = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Max(initialXPosition + player.transform.position.x * fractionOfPlayerPosition, initialXPosition),
            transform.position.y,
            transform.position.z
        );
    }
}
