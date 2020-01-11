using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int gems = 0;
    public int stars = 0;
    private readonly Dictionary<Ability, float> timers = new Dictionary<Ability, float>()
    {
        { Ability.MOVE_LEFT, 0f },
        { Ability.MOVE_RIGHT, 0f },
        { Ability.JUMP, 0f },
        { Ability.DOUBLE_JUMP, 0f },
        { Ability.DICE_GUN, 0f },
    };

    void Update()
    {
        var keys = new List<Ability>(timers.Keys);
        foreach (var key in keys)
        {
            timers[key] = Mathf.Max(0, timers[key] - Time.deltaTime);
        }
    }

    public bool TryActivate(Ability ability)
    {
        if (!CanActivate(ability))
        {
            return false;
        }

        timers[ability] = 30f;
        //gems--;
        return true;
    }

    private bool CanActivate(Ability ability)
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
        return timers[ability];
    }

    public bool IsAbilityActive(Ability ability)
    {
        return timers[ability] > 0;
    }
}
