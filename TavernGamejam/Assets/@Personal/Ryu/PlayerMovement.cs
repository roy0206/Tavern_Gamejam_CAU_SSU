using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : Module
{
    Player player;
    float playerHeight;
    bool onLand;
    bool isSit;
    float dashCurtime = 0;
    bool allowJump = true;

    Dictionary<string, Vector2> outerForce = new();
    public PlayerMovement(MonoThing thing) : base(thing) { player = (Player)thing; onLand = true; playerHeight = ((CapsuleCollider2D)player.Collider).size.y; }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        LandMove();
        WaterMove();

        outerForce.Clear();

        dashCurtime += TimeManager.FixedDeltaTime;
    }

    public void SetLand()
    {
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Dash);
        UserInput.Instance.BindKeyDown(KeyCode.Space, Jump);
        UserInput.Instance.BindKeyDown(KeyCode.S, SitDown);
        UserInput.Instance.BindKeyUp(KeyCode.S, SitUp);
        onLand = true;
        player.Rigidbody.freezeRotation = true;
        player.Rigidbody.linearDamping = 0;
        player.transform.DORotate(new Vector3(0, 0, 0), 1f);
        player.Animator.SetBool("Swim", false);
    }

    public void SetWater()
    {
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Jump);
        UserInput.Instance.BindKeyDown(KeyCode.Space, Dash);
        UserInput.Instance.UnbindKeyDown(KeyCode.S, SitDown);
        UserInput.Instance.UnbindKeyUp(KeyCode.S, SitUp);
        onLand = false;
        player.Rigidbody.freezeRotation = false;
        player.Rigidbody.linearDamping = 4;
        SitUp();
        player.Animator.SetBool("Walk", false);
    }

    
    void LandMove()
    {
        if (!onLand) return;
        float moveX = new Vector2(UserInput.Instance.MoveDirectionRaw.x, 0).normalized.x * TimeManager.TImeScale * player.BaseSpeed * (isSit? 0.6f : 1);
        player.Rigidbody.linearVelocity = new Vector2(moveX, player.Rigidbody.linearVelocity.y);

        if(Mathf.Abs(moveX)>0) player.Animator.SetBool("Walk", true);
        else player.Animator.SetBool("Walk", false);
        foreach (var force in outerForce.Values)
        {
            player.Rigidbody.linearVelocity += force;
        }

    }
    void WaterMove()
    {
        if (onLand) return;
        Vector2 dir = UserInput.Instance.MoveDirectionRaw;

        if(dir.sqrMagnitude > 0)
        {
            Quaternion targetRot = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
            player.transform.DORotateQuaternion(targetRot, 0.5f);
            player.Rigidbody.AddForce(dir * player.BaseSpeed * TimeManager.TImeScale * 0.05f, ForceMode2D.Impulse);
            player.Animator.SetBool("Swim", true);
        }
        else
        {
            player.Animator.SetBool("Swim", false);
        }
        if (dir.x < 0) player.SpriteRenderer.flipX = true;
        else if(dir.x > 0) player.SpriteRenderer.flipX = false;

        Float();

        foreach (var force in outerForce.Values)
        {
            player.Rigidbody.AddForce(force, ForceMode2D.Force);
        }
    }

    public void Slip()
    {
        player.StartCoroutine(DoSlip());
    }
    IEnumerator DoSlip()
    {
        allowJump = false;
        float curTime = 0;
        float moveX = new Vector2(UserInput.Instance.MoveDirectionRaw.x, 0).normalized.x;
        while (Physics2D.Raycast(player.transform.position, - player.transform.up, playerHeight / 2 + 0.1f, LayerMask.GetMask("Floor")))
        {
            outerForce["Slip"] = new Vector2(moveX * 10, 0);
            yield return null;
        }
        while(curTime < 1)
        {
            outerForce["Slip"] = new Vector2(moveX * 10, 0);
            curTime += TimeManager.DeltaTime;
            yield return null;
        }
        allowJump = true;
    }

    public void Jump()
    {
        if (!allowJump || isSit) return;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, -player.transform.up, playerHeight / 2 + 0.1f, LayerMask.GetMask("Floor"));
        if(hit)
            player.Rigidbody.AddForce(Vector2.up * player.JumpPower, ForceMode2D.Impulse);
    }

    public void Dash()
    {
        if(dashCurtime > player.DashCooltime)
        {
            player.Rigidbody.AddForce(player.transform.up * player.DashPower, ForceMode2D.Impulse);
            dashCurtime = 0;
            player.Animator.SetTrigger("Dash");
        }
    }

    public void SitDown()
    {
        isSit = true;
        ((CapsuleCollider2D)player.Collider).size = new Vector2(1, 1.5f);
        player.Animator.SetBool("Sit", true);
    }

    public void SitUp()
    {
        isSit = false;
        ((CapsuleCollider2D)player.Collider).size = new Vector2(1, 2f);
        player.Animator.SetBool("Sit", false);
    }
    public override void OnRemoved()
    {
        base.OnRemoved();
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Jump);
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Dash);
        UserInput.Instance.UnbindKeyDown(KeyCode.S, SitDown);
        UserInput.Instance.UnbindKeyUp(KeyCode.S, SitUp);
    }


    void Float()
    {
        float floatForce = Mathf.Abs(Mathf.Cos((player.transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad) * playerHeight) * 5 + 10;
        player.Rigidbody.AddForce(Vector2.up * 10, ForceMode2D.Force);
    }
}
