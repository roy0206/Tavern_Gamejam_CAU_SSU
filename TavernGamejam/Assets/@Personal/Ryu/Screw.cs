using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class Screw : Entity_baseclass
{
    Player player;
    [SerializeField] float pullPower;
    [SerializeField] float maxDistance;
    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        AudioManager.Instance.PlaySound("Motor", transform, 1, 9999);
    }

    new protected void FixedUpdate()
    {
        base.FixedUpdate();
        if (player.GetModule<StateMachine<Player>>() == null) return;
        if (player.GetModule<StateMachine<Player>>().GetCurrentState() != typeof(PlayerWaterState)) return;

        Vector2 pullVec = -(player.transform.position - transform.position);
        if (pullVec.magnitude > maxDistance) return;
        if (pullVec.magnitude < 0.25f) player.Dead(DeathType.Ground);
        player.Rigidbody.AddForce(pullVec * pullPower / pullVec.magnitude, ForceMode2D.Force);
        
    }
}

