using UnityEngine;

public class PlayerWaterState : State<Player>
{
    public override void Enter(Player player)
    {
        player.GetModule<PlayerMovement>().SetWater();
        Debug.Log("Enter Water");
    }

    public override void Execute(Player player)
    {
        if (player.transform.position.y > player.SurfaceLevel)
        {
            player.GetModule<StateMachine<Player>>().ChangeState("Land");
        }
        
    }

    public override void Exit(Player player)
    {

    }
}