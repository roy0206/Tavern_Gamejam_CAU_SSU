using DG.Tweening;
using UnityEngine;

public class PlayerMovement : Module
{
    Player player;
    float playerHeight;
    bool onLand;
    float dashCurtime = 0;
    public PlayerMovement(MonoThing thing) : base(thing) { player = (Player)thing; onLand = true; playerHeight = ((CapsuleCollider2D)player.Collider).size.y; }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        LandMove();
        WaterMove();

        dashCurtime += TimeManager.FixedDeltaTime;
    }

    public void SetLand()
    {
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Dash);
        UserInput.Instance.BindKeyDown(KeyCode.Space, Jump);
        onLand = true;
        player.Rigidbody.freezeRotation = true;
        player.Rigidbody.linearDamping = 0;
        player.transform.DORotate(new Vector3(0, 0, 0), 1f);
    }

    public void SetWater()
    {
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Jump);
        UserInput.Instance.BindKeyDown(KeyCode.Space, Dash);
        onLand = false;
        player.Rigidbody.freezeRotation = false;
        player.Rigidbody.linearDamping = 4;
    }

    
    void LandMove()
    {
        if (!onLand) return;
        float moveX = new Vector2(UserInput.Instance.MoveDirectionRaw.x, 0).normalized.x * TimeManager.TImeScale * player.BaseSpeed;
        player.Rigidbody.linearVelocity = new Vector2(moveX, player.Rigidbody.linearVelocity.y);
    }
    void WaterMove()
    {
        if (onLand) return;
        Vector2 dir = UserInput.Instance.MoveDirectionRaw;

        if(dir.sqrMagnitude > 0)
        {
            Quaternion targetRot = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
            player.transform.DORotateQuaternion(targetRot, 0.5f);
            player.Rigidbody.AddForce(dir * player.BaseSpeed * TimeManager.TImeScale * 0.1f, ForceMode2D.Impulse);
        }
        Float();
    }

    public void Jump()
    {
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
        }
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Jump);
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Dash);
    }


    void Float()
    {
        float floatForce = Mathf.Abs(Mathf.Cos((player.transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad) * playerHeight) * 5 + 10;
        player.Rigidbody.AddForce(Vector2.up * 10, ForceMode2D.Force);
    }
}
