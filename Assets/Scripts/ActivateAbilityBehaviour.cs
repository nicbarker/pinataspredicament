using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public enum AbilityState
{
  ENTERING,
  ACTIVE,
  EXITING,
  EXITED,
}

public class AbilityButton
{
  public Ability ability;
  public AbilityState abilityState = AbilityState.EXITED;
  public GameObject button;

  public float tweenState;
  public float previousXPosition;
}

public class ActivateAbilityBehaviour : MonoBehaviour
{
  private GameData gameData;
  private AbilityButton[] activeAbilities = new AbilityButton[Enum.GetNames(typeof(Ability)).Length];
  private int activeAbilitiesCount;
  public GameObject UIButtonPrefab;
  static int TOP_OFFSET = 600;
  static int BUTTON_GUTTER = 16;
  static float TWEEN_INCREMENT = 0.05f;
  static float BUTTON_SCALE_FACTOR = 1.7f;


  void Start()
  {
    this.gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
    int i = 0;
    foreach (KeyValuePair<Ability, float> entry in gameData.abilityTimers)
    {
      AbilityButton abilityButton = new AbilityButton();
      abilityButton.button = Instantiate(UIButtonPrefab, this.gameObject.transform);
      var abilityImage = abilityButton.button.GetComponent<Image>();
      abilityImage.color = new Color(255, 255, 255, 0);
      abilityImage.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(AbilityExtensions.GetImagePath(entry.Key), typeof(Sprite));
      abilityImage.transform.localScale = new Vector3(0f, 0f, 0f);
      abilityButton.button.GetComponent<RectTransform>().localPosition = new Vector3(0, TOP_OFFSET, 0);

      abilityButton.abilityState = AbilityState.EXITED;
      abilityButton.ability = entry.Key;
      activeAbilities[i] = abilityButton;
      i++;
    }
  }

  void Update()
  {
    int activeIndex = 0;
    for (int i = 0; i < this.activeAbilities.Length; i++)
    {
      var activeAbility = this.activeAbilities[i];
      // Set to EXITING when ability has expired
      if (activeAbility.abilityState == AbilityState.ACTIVE && gameData.abilityTimers[activeAbility.ability] <= 0)
      {
        activeAbility.abilityState = AbilityState.EXITING;
        activeAbilitiesCount--;
        activeAbility.tweenState = 0;
        activeAbility.button.GetComponentInChildren<ParticleSystem>().Stop();
      }
      // Set to ENTERING when ability is first activated
      else if (activeAbility.abilityState == AbilityState.EXITED && gameData.abilityTimers[activeAbility.ability] > 0)
      {
        activeAbility.abilityState = AbilityState.ENTERING;
        activeAbility.tweenState = 0;
        activeAbilitiesCount++;
      }

      // Update tween positions of entering and active abilities
      if (activeAbility.abilityState == AbilityState.ACTIVE)
      {
        RectTransform transform = activeAbility.button.GetComponent<RectTransform>();
        float localWidth = (transform.rect.width * BUTTON_SCALE_FACTOR);
        float newPosition = (activeIndex * (localWidth + BUTTON_GUTTER)) -
          (
            (activeAbilitiesCount - 1) *
            ((localWidth + BUTTON_GUTTER) * 0.5f)
          );

        if (newPosition != transform.localPosition.x)
        {
          transform.localPosition = new Vector3(Mathf.Lerp(activeAbility.previousXPosition, newPosition, activeAbility.tweenState), TOP_OFFSET, 0);
          activeAbility.tweenState += TWEEN_INCREMENT * Time.deltaTime * 100;
        }
        else
        {
          activeAbility.previousXPosition = newPosition;
          activeAbility.tweenState = 0;
        }
        if (activeAbility.button.GetComponentsInChildren<Image>()[1].fillAmount != (float)Math.Round(1 - gameData.RemainingFractionFor(activeAbility.ability), 3))
        {
          activeAbility.button.GetComponentInChildren<ParticleSystem>().Play();
          activeAbility.button.GetComponentsInChildren<Image>()[1].fillAmount = (float)Math.Round(1 - gameData.RemainingFractionFor(activeAbility.ability), 3);
        }
        else
        {
          activeAbility.button.GetComponentInChildren<ParticleSystem>().Stop();
        }
        activeIndex++;
      }
      else if (activeAbility.abilityState == AbilityState.ENTERING)
      {
        RectTransform transform = activeAbility.button.GetComponent<RectTransform>();
        float localWidth = (transform.rect.width * BUTTON_SCALE_FACTOR);
        float newPosition = (activeIndex * (localWidth + BUTTON_GUTTER)) -
          (
            (activeAbilitiesCount - 1) *
            ((localWidth + BUTTON_GUTTER) * 0.5f)
          );
        transform.localPosition = new Vector3(newPosition, TOP_OFFSET, 0);

        if (activeAbility.tweenState < 1)
        {
          activeAbility.button.GetComponent<Image>().color = new Color(255, 255, 255, activeAbility.tweenState);
          activeAbility.button.transform.localScale = new Vector3(BUTTON_SCALE_FACTOR * activeAbility.tweenState, BUTTON_SCALE_FACTOR * activeAbility.tweenState, BUTTON_SCALE_FACTOR * activeAbility.tweenState);
          activeAbility.tweenState += TWEEN_INCREMENT * Time.deltaTime * 100;
        }
        else
        {
          activeAbility.tweenState = 0;
          activeAbility.abilityState = AbilityState.ACTIVE;
        }
        activeAbility.button.GetComponentsInChildren<Image>()[1].fillAmount = gameData.RemainingFractionFor(activeAbility.ability);
        activeIndex++;
      }
      else if (activeAbility.abilityState == AbilityState.EXITING)
      {
        if (activeAbility.tweenState < 1)
        {
          var invertedTweenState = 1 - activeAbility.tweenState;
          activeAbility.button.GetComponent<Image>().color = new Color(255, 255, 255, invertedTweenState);
          activeAbility.button.transform.localScale = new Vector3(BUTTON_SCALE_FACTOR * invertedTweenState, BUTTON_SCALE_FACTOR * invertedTweenState, BUTTON_SCALE_FACTOR * invertedTweenState);
          activeAbility.tweenState += TWEEN_INCREMENT * Time.deltaTime * 100;
        }
        else
        {
          activeAbility.tweenState = 0;
          activeAbility.abilityState = AbilityState.EXITED;
        }
      }
    }
  }
}
