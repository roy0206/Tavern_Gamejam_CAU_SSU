using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class PlasticBagEffect : Effect
{
    public PlasticBagEffect(Effector effector) : base(effector)
    {
        maxDuration = int.MaxValue;
        duration = maxDuration;
    }
    public override void StartEffect()
    {
        base.StartEffect();
        effector.Thing.GetModule<Oxygen>().AddChange("PlasticBag", -26.5f);

        Debug.Log("PlasticBagEffect Added");
    }

    public override void EndEffect()
    {
        base.EndEffect();
        effector.Thing.GetModule<Oxygen>().RemoveChange("PlasticBag");
    }

}

