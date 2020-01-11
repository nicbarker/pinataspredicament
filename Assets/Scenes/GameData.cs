using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int gems = 0;
    private readonly Dictionary<Ability, float> timers = new Dictionary<Ability, float>()
    {
        { Ability.MOVE_LEFT, 30f },
        { Ability.MOVE_RIGHT, 30f },
        { Ability.JUMP, 30f },
        { Ability.DOUBLE_JUMP, 30f }
    };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
