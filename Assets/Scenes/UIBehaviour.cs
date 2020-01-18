using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        this.gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.FindWithTag("GemCount").GetComponent<Text>().text = gameData.gems.ToString();
        GameObject.FindWithTag("StarCount").GetComponent<Text>().text = gameData.stars.ToString();
    }
}
