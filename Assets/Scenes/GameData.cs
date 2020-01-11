using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int gems = 0;
    public int stars = 0;
    private readonly Dictionary<Ability, float> timers = new Dictionary<Ability, float>()
    {
        { Ability.MOVE_LEFT, 0 },
        { Ability.MOVE_RIGHT, 0 },
        { Ability.JUMP, 0 }
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
        int abilitiesActive = 0;
        foreach (KeyValuePair<Ability, float> entry in timers)
        {
            abilitiesActive += entry.Value > 0 ? 1 : 0;
        }

        if (true || gems > abilitiesActive)
        {
            timers[ability] = 30f;
            //gems--;
            return true;
        }

        return false;
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
