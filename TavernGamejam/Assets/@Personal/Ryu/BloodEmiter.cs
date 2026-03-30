using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class BloodEmiter : Module
{
    ParticleSystem ps;
    public BloodEmiter(MonoThing thing) : base(thing)
    {
        ps = Thing.transform.Find("Blood").GetComponent<ParticleSystem>();
    }

    public void Bleed(int amount, bool useGravity)
    {
        var main = ps.main;
        main.gravityModifier = useGravity ? 1 : 0;
        ps.Emit(amount);
    }
}
