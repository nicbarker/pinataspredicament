using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateAbilityBehaviour : MonoBehaviour
{
    public GameData gameData;
    public string abilityKey;
    public string abilityLabel;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => gameData.abilityTimers[abilityKey] = 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.abilityTimers[abilityKey] > 0)
        {
            GetComponentInChildren<Text>().text = abilityLabel + " (" + (int)gameData.abilityTimers[abilityKey] + ")";
        }
        else
        {
            GetComponentInChildren<Text>().text = abilityLabel;
        }
    }
}
