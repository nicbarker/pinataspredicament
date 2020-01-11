﻿using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static float INITIAL_ABILITY_TIMER = 30f;

    public static Dictionary<Ability, float> MakeAbilityTimers()
    {
        var timers = new Dictionary<Ability, float>();
        foreach (Ability ability in System.Enum.GetValues(typeof(Ability)))
        {
            timers.Add(ability, INITIAL_ABILITY_TIMER);
        }
        return timers;
    }

    public int gems = 0;
    public int stars = 0;
    private readonly Dictionary<Ability, float> abilityTimers = MakeAbilityTimers();

    void Update()
    {
        var keys = new List<Ability>(abilityTimers.Keys);
        foreach (var key in keys)
        {
            abilityTimers[key] = Mathf.Max(0, abilityTimers[key] - Time.deltaTime);
        }
    }

    public bool TryActivate(Ability ability)
    {
        if (!CanActivate(ability))
        {
            return false;
        }

        abilityTimers[ability] = 30f;
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
        return abilityTimers[ability];
    }

    public bool IsAbilityActive(Ability ability)
    {
        return abilityTimers[ability] > 0;
    }
}
