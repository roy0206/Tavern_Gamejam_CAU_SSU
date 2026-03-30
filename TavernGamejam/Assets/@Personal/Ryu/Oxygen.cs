using System;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : Module
{
    Player player;
    SpriteRenderer red;
    private float oxygen;
    private float maxOxygen;
    Dictionary<string, float> changeOfOxygen;

    public float Value => oxygen;
    public Oxygen(MonoThing thing) : base(thing)
    {
        player = (Player)thing;
        red = player.transform.Find("Red").GetComponent<SpriteRenderer>();
        changeOfOxygen = new();
    }
    public override void Init(params object[] objects)
    {
        base.Init(objects);
        oxygen = (float)objects[0];
        maxOxygen = oxygen;
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        foreach (float v in changeOfOxygen.Values)
        {
            oxygen += v * TimeManager.FixedDeltaTime;
        }
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);
        if(maxOxygen == 0)
        {
            player.Dead(DeathType.Suffocated);
        }
        red.transform.localPosition = new Vector3(0, Mathf.Lerp(0, 0.5f, 1 - oxygen / maxOxygen),0);
        red.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1f, 1 - oxygen / maxOxygen), 1);

        Debug.Log(oxygen);
    }

    public void AddChange(string name, float value)
    {
        changeOfOxygen[name] = value;
    }
    public void RemoveChange(string name)
    {
        changeOfOxygen.Remove(name);
    }
}

