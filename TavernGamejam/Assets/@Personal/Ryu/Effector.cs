using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Effector : Module
{
    List<Effect> effects = new();
    List<Effect> removingEfects = new();
    public Effector(MonoThing thing) : base(thing)
    {

    }

    public void AddEffect<T>()
    {
        Effect newEffect = Activator.CreateInstance(typeof(T), new object[] {this}) as Effect;
        effects.Add(newEffect);
        newEffect.StartEffect();
    }

    public bool HasEffect<T>()
    {
        if (effects.Find(e => e.GetType() == typeof(T)) == null)
            return false;
        return true;
    }

    public void RemoveEffect<T>()
    {
        var effect = effects.Find(e => e.GetType() == typeof(T));
        if (effect == null) return;

        removingEfects.Add(effect);
    }

    public override void Init(params object[] objects) { }

    public override void OnAdded()
    {
        base.OnAdded();
        effects = new List<Effect>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        foreach (Effect effect in effects)
        {
            effect.UpdateEffect();
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        foreach (Effect effect in effects)
        {
            effect.FixedUpdateEffect();
        }
        foreach (Effect effect in effects)
        {
            effect.duration -= TimeManager.FixedDeltaTime;
            if(effect.duration <= 0) removingEfects.Add(effect);
        }

        while (removingEfects.Count > 0)
        {
            effects[0].EndEffect();
            effects.Remove(removingEfects[0]);
            removingEfects.RemoveAt(0);
        }

    }
}
