using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
  public float ABILITY_DURATION = 10f;

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
  public int totalGems;
  public int stars = 0;
  public readonly Dictionary<Ability, float> abilityTimers = MakeAbilityTimers();
  private bool hasActivatedAbility = false;

  public static int[][] levelStarRanks = {
    new int[]{ 0, 0, 0, 0 }, // Menu scene
    new int[]{ 5, 6, 7, 8 }, // Level 1
    new int[]{ 8, 9, 10, 11 }, // Level 2
  };

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

  public void IncrementGems()
  {
    gems++;
    totalGems++;
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

  public string getStarRank(int levelIndex)
  {
    int rankIndex = 0;
    for (int i = 1; i < levelStarRanks[levelIndex].Length; i++)
    {
      if (stars >= levelStarRanks[levelIndex][i])
      {
        rankIndex = i;
      }
    }

    switch (rankIndex)
    {
      case 0: return "C";
      case 1: return "B";
      case 2: return "A";
      case 3: return "A+";
      default: return "???";
    }
  }
}
