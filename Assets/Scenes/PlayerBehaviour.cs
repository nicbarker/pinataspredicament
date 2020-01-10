using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float basePlayerSpeed = 10;

    private bool inContactWithGround = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * basePlayerSpeed;
        if (System.Math.Abs(speed) > 0.001f)
        {
            GetComponent<SpriteRenderer>().flipX = speed < 0;
            transform.position += new Vector3(speed, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && inContactWithGround)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2500));
            inContactWithGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        inContactWithGround = true;
    }
}
