using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int gems = 0;
    public Dictionary<string, float> abilityTimers = new Dictionary<string, float>()
    {
        { "moveLeft", 0f },
        { "moveRight", 0f },
        { "jump", 0f }
    };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        List<string> keys = new List<string>(abilityTimers.Keys);
        foreach (string key in keys)
        {
            abilityTimers[key] = Mathf.Max(0, abilityTimers[key] - Time.deltaTime);
        }
    }
}
