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
    private readonly Dictionary<Ability, float> abilityTimers = MakeAbilityTimers();
    private bool hasActivatedAbility = false;

    void Update()
    {
        var keys = new List<Ability>(abilityTimers.Keys);
        foreach (var key in keys)
        {
            abilityTimers[key] = Mathf.Max(0, abilityTimers[key] - Time.deltaTime);
        }
    }

    public bool IsActiveOrTryActivate(Ability ability)
    {
        if (IsAbilityActive(ability))
        {
            return true;
        }

        return TryActivate(ability);
    }

    public bool TryActivate(Ability ability)
    {
        if (!CanActivate(ability))
        {
            return false;
        }
        hasActivatedAbility = true;
        abilityTimers[ability] = ABILITY_DURATION;
        gems--;
        return true;
    }

    public bool IsGameOver()
    {
        return hasActivatedAbility && gems <= 0 && !AnyAbilityActive();
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

    public bool CanActivate(Ability ability)
    {
        if (gems <= 0)
        {
            return false;
        }

        switch (ability)
        {
            case Ability.JUMP:
            case Ability.MOVE_LEFT:
            case Ability.MOVE_RIGHT:
            case Ability.DICE_GUN:
                return true;
            case Ability.DOUBLE_JUMP:
                return IsAbilityActive(Ability.JUMP);
            default:
                throw new System.Exception($"Unknown ability {ability.ToString()}");
        }
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
