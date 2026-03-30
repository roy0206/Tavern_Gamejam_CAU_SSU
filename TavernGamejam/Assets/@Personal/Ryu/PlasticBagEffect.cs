using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class PlasticBagEffect : Effect
{
    SpriteRenderer bag;
    GameObject prefab;
    Player player;
    public PlasticBagEffect(Effector effector) : base(effector)
    {
        maxDuration = int.MaxValue;
        duration = maxDuration;
        prefab = Resources.Load<GameObject>("Prefabs/PlasticBagCovered");
        player = effector.Thing as Player;
    }
    public override void StartEffect()
    {
        base.StartEffect();
        effector.Thing.GetModule<Oxygen>().AddChange("PlasticBag", -26.5f);
        bag = Cloner.Instance.Clone<SpriteRenderer>(prefab, effector.Thing.transform.position, Quaternion.identity, effector.Thing.transform);
        bag.transform.localRotation = Quaternion.Euler(0, 0, 0);

        Debug.Log("PlasticBagEffect Added");
    }

    public override void EndEffect()
    {
        base.EndEffect();
        effector.Thing.GetModule<Oxygen>().RemoveChange("PlasticBag");
        bag.transform.parent = bag.transform.parent.parent;
        bag.DOFade(0, 1).OnComplete(() => Cloner.Destroy(bag.gameObject));
    }

}

