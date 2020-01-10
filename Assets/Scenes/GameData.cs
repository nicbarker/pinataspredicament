using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int gems = 0;
    public float moveLeftTimer = 30f;
    public float moveRightTimer = 30f;
    public float jumpTimer = 30f;
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
