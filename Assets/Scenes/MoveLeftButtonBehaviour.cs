using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLeftButtonBehaviour : MonoBehaviour
{
    public GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => gameData.moveLeftTimer = 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.moveLeftTimer > 0)
        {
            GetComponentInChildren<Text>().text = "Left (" + (int)gameData.moveLeftTimer + ")";
        }
        else
        {
            GetComponentInChildren<Text>().text = "Left";
        }
    }
}
