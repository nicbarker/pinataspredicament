using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    public float floatTimer = 3.0f;
    public float verticalFloatDistance = 0.5f;
    public float movementSpeed = 0.1f;
    public bool movingLeft = true;

    private bool downwards = true;
    private float currentFloatTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentFloatTimer = floatTimer;
    }

    // Update is called once per frame
    void Update()
    {
        currentFloatTimer -= Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x + (movementSpeed * Time.deltaTime) * (movingLeft ? -1 : 1),
            transform.position.y + (verticalFloatDistance * (Time.deltaTime / floatTimer)) * (downwards ? -1 : 1),
            transform.position.y
        );

        if (currentFloatTimer < 0)
        {
            downwards = !downwards;
            currentFloatTimer = floatTimer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch ((Layers)collision.gameObject.layer)
        {
            case Layers.EdgeCollider:
                movingLeft = !movingLeft;
                GetComponent<SpriteRenderer>().flipX = !movingLeft;
                return;
            default:
                return;
        }
    }
}
