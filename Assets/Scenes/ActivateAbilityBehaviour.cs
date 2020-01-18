using UnityEngine;
using UnityEngine.UI;

public class ActivateAbilityBehaviour : MonoBehaviour
{
    private GameData gameData;
    public Ability ability;
    public Image overlay;

    private Button button;

    void Start()
    {
        this.gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => gameData.TryActivate(ability));
    }

    void Update()
    {
        if (gameData.IsAbilityActive(ability))
        {
            button.interactable = true;
            overlay.fillAmount = gameData.RemainingFractionFor(ability);
        }
        else
        {
            button.interactable = gameData.CanActivate(ability);
            overlay.fillAmount = 0f;
        }
    }
}
