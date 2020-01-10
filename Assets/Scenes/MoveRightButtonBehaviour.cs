using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveRightButtonBehaviour : MonoBehaviour
{
    public GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => gameData.moveRightTimer = 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.moveRightTimer > 0)
        {
            GetComponentInChildren<Text>().text = "Right (" + (int)gameData.moveRightTimer + ")";
        }
        else
        {
            GetComponentInChildren<Text>().text = "Right";
        }
    }
}
