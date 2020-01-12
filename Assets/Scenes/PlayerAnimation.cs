using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play("idleAnimation");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
