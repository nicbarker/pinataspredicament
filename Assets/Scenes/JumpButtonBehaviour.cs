using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpButtonBehaviour : MonoBehaviour
{
    public GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => gameData.jumpTimer = 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.jumpTimer > 0)
        {
            GetComponentInChildren<Text>().text = "Jump (" + (int)gameData.jumpTimer + ")";
        } else
        {
            GetComponentInChildren<Text>().text = "Jump";
        }
    }
}
