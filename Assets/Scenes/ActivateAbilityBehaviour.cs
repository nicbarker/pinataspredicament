using UnityEngine;
using UnityEngine.UI;

public class ActivateAbilityBehaviour : MonoBehaviour
{
    public GameData gameData;
    public Ability ability;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => gameData.TryActivate(ability));
    }

    void Update()
    {
        if (gameData.IsAbilityActive(ability))
        {
            GetComponentInChildren<Text>().text = ability.GetLabel() + " (" + (int)gameData.RemainingTimeFor(ability) + ")";
        }
        else
        {
            GetComponentInChildren<Text>().text = ability.GetLabel();
        }
    }
}
