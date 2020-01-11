using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int gems = 0;
    private readonly Dictionary<Ability, float> timers = new Dictionary<Ability, float>()
    {
        { Ability.MOVE_LEFT, 30f },
        { Ability.MOVE_RIGHT, 30f },
        { Ability.JUMP, 30f }
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
        if (gems > 0)
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
