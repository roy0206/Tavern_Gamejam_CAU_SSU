using System;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : Module
{
    Player player;
    PlayerVisuals visual;
    private float oxygen;
    private float maxOxygen;
    Dictionary<string, float> changeOfOxygen;

    public float Value => oxygen;
    public Oxygen(MonoThing thing) : base(thing)
    {
        player = (Player)thing;
        /*        red = player.transform.Find("Red").GetComponent<SpriteRenderer>();*/
        visual = player.GetComponent<PlayerVisuals>();
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
        if(oxygen == 0)
        {
            player.Dead(DeathType.Suffocated);
        }
        /*        red.transform.localPosition = new Vector3(0, Mathf.Lerp(0, 0.5f, 1 - oxygen / maxOxygen),0);
                red.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1f, 1 - oxygen / maxOxygen), 1);*/
        visual.headTint = new Color(1, oxygen / maxOxygen, oxygen / maxOxygen);
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

