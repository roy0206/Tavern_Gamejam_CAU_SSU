using UnityEngine;

public class PlayerMovement : Module
{
    Player player;
    bool onLand;
    public PlayerMovement(MonoThing thing) : base(thing) { player = (Player)thing; onLand = true; }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        LandMove();
        WaterMove();
    }

    public void SetLand()
    {
        UserInput.Instance.BindKeyDown(KeyCode.Space, Jump);
        onLand = true;
    }

    public void SetWater()
    {
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, Jump);
        onLand = false;
    }

    
    public void LandMove()
    {
        if (!onLand) return;
        float moveX = new Vector2(UserInput.Instance.MoveDirection.x, 0).normalized.x;
        player.Rigidbody.linearVelocity = new Vector2(moveX, player.Rigidbody.linearVelocity.y) * player.BaseSpeed * TimeManager.FixedDeltaTime;
    }
    public void WaterMove()
    {
        if (onLand) return;
        player.Rigidbody.AddForce(UserInput.Instance.MoveDirection * player.BaseSpeed * TimeManager.FixedDeltaTime, ForceMode2D.Force);
    }

    public void Jump()
    {
        player.Rigidbody.AddForce(Vector2.up * player.JumpPower, ForceMode2D.Force);
    }
}
