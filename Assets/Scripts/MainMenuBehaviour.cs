using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuBehaviour : MonoBehaviour
{
    static int WORLD_COUNT = 3;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("WorldsButton").GetComponent<Button>().onClick.AddListener(() => {
            transform.Find("Worlds").GetComponent<Canvas>().enabled = true;
            transform.Find("MainMenu").GetComponent<Canvas>().enabled = false;
        });
        for (int i = 0; i < WORLD_COUNT; i++) {
            GameObject.Find("World" + (i + 1).ToString() + "Button").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("SceneChanger").GetComponent<SceneChangerBehaviour>().FadeToScene(i);
            });
        }
    }
}
