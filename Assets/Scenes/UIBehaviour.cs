using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    public GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.FindWithTag("GemCount").GetComponent<Text>().text = gameData.gems.ToString();
        GameObject.FindWithTag("StarCount").GetComponent<Text>().text = gameData.stars.ToString();
    }
}
