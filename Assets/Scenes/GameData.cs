using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int gems = 0;
    public float moveLeftTimer = 0;
    public float moveRightTimer = 0;
    public float jumpTimer = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveLeftTimer = Mathf.Max(0, moveLeftTimer - Time.deltaTime);
        moveRightTimer = Mathf.Max(0, moveRightTimer - Time.deltaTime);
        jumpTimer = Mathf.Max(0, jumpTimer - Time.deltaTime);
    }
}
