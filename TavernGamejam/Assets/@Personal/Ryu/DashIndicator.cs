using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class DashIndicator : Module
{
    Player plr;
    Image img;
    public DashIndicator(MonoThing thing) : base(thing)
    {
        plr = (Player)thing;
        img = plr.transform.Find("Canvas/Image").GetComponent<Image>();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        img.fillAmount = Mathf.Clamp(plr.GetModule<PlayerMovement>().DashCurtime / plr.DashCooltime, 0, 1);
        if (img.fillAmount == 1) img.color = Color.white;
        else img.color = new Color(0.4f, 0.4f, 0.4f);

    }

    public void SetIndicator(bool v)
    {
        img.gameObject.SetActive(v);
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        SetIndicator(false);
    }
}

