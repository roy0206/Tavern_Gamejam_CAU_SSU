using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlasticBag : Entity_baseclass
{
    Player player;
    [SerializeField] float detectRange;
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    new protected void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = (player.transform.position - transform.position).magnitude;
        if(distance < detectRange)
        {
            player.GetModule<Effector>().AddEffect<PlasticBagEffect>();
            Destroy(gameObject);
        }
    }
}

