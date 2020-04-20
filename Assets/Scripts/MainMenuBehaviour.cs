using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("WorldsButton").GetComponent<Button>().onClick.AddListener(() => {
            transform.Find("Worlds").GetComponent<Canvas>().enabled = true;
            transform.Find("MainMenu").GetComponent<Canvas>().enabled = false;
        });
        GameObject.Find("World1Button").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("SceneChanger").GetComponent<SceneChangerBehaviour>().FadeToScene(1);
        });
        GameObject.Find("World2Button").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("SceneChanger").GetComponent<SceneChangerBehaviour>().FadeToScene(2);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
