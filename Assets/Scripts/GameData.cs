using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
  public static float ABILITY_DURATION = 10f;

  public static Dictionary<Ability, float> MakeAbilityTimers()
  {
    var timers = new Dictionary<Ability, float>();
    foreach (Ability ability in System.Enum.GetValues(typeof(Ability)))
    {
      timers.Add(ability, 0f);
    }
    return timers;
  }

  public int gems = 0;
  public int stars = 0;
  public readonly Dictionary<Ability, float> abilityTimers = MakeAbilityTimers();
  private bool hasActivatedAbility = false;

  public bool TryUseAbility(Ability ability)
  {
    if (!IsAbilityActive(ability))
    {
      if (gems <= 0)
      {
        return false;
      }
      hasActivatedAbility = true;
      abilityTimers[ability] = ABILITY_DURATION;
      gems--;
    }
    switch (ability)
    {
      case Ability.MOVE_LEFT:
      case Ability.MOVE_RIGHT:
        {
          abilityTimers[ability] -= 0.01f;
          break;
        }
      case Ability.JUMP:
      case Ability.DOUBLE_JUMP:
      case Ability.DICE_GUN:
        {
          abilityTimers[ability] -= ABILITY_DURATION / 3 + 0.05f;
        }
        break;
    }
    return true;
  }

  private bool AnyAbilityActive()
  {
    foreach (Ability ability in System.Enum.GetValues(typeof(Ability)))
    {
      if (IsAbilityActive(ability))
      {
        return true;
      }
    }
    return false;
  }

  public float RemainingTimeFor(Ability ability)
  {
    return abilityTimers[ability];
  }

  public float RemainingFractionFor(Ability ability)
  {
    return RemainingTimeFor(ability) / ABILITY_DURATION;
  }

  public bool IsAbilityActive(Ability ability)
  {
    return abilityTimers[ability] > 0;
  }
}
