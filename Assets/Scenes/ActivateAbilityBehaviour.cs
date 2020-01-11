using UnityEngine;
using UnityEngine.UI;

public class ActivateAbilityBehaviour : MonoBehaviour
{
    public GameData gameData;
    public Ability ability;
    public string abilityLabel;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => gameData.abilityStore.Activate(ability));
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.abilityStore.IsActive(ability))
        {
            GetComponentInChildren<Text>().text = abilityLabel + " (" + (int)gameData.abilityStore.RemainingTimeFor(ability) + ")";
        }
        else
        {
            GetComponentInChildren<Text>().text = abilityLabel;
        }
    }
}
