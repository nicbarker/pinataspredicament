using UnityEngine;
using UnityEngine.UI;

public class ActivateAbilityBehaviour : MonoBehaviour
{
    public GameData gameData;
    public Ability ability;
    public Image overlay;

    private Button button;

    void Start()
    {
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
