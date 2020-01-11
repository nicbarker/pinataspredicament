﻿using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public class AbilityStore
    {
        private readonly Dictionary<Ability, float> timers = new Dictionary<Ability, float>()
        {
            { Ability.MOVE_LEFT, 0f },
            { Ability.MOVE_RIGHT, 0f },
            { Ability.JUMP, 0f }
        };

        public bool IsActive(Ability ability)
        {
            return timers[ability] > 0;
        }

        public void Activate(Ability ability)
        {
            timers[ability] = 30f;
        }

        public float RemainingTimeFor(Ability ability)
        {
            return timers[ability];
        }

        public void Tick()
        {
            var keys = new List<Ability>(timers.Keys);
            foreach (var key in keys)
            {
                timers[key] = Mathf.Max(0, timers[key] - Time.deltaTime);
            }
        }
    }

    public int gems = 0;
    public AbilityStore abilityStore = new AbilityStore();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        abilityStore.Tick();
    }
}