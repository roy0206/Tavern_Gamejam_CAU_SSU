using UnityEngine;
public class PlayerLandState : State<Player>
{
    public override void Enter(Player player)
    {
        player.GetModule<PlayerMovement>().SetLand();
        Debug.Log("Enter Land");
    }

    public override void Execute(Player player)
    {
        if (player.transform.position.y < player.SurfaceLevel)
        {
            player.GetModule<StateMachine<Player>>().ChangeState("Water");
        }
        
    }

    public override void Exit(Player player)
    {

    }
}