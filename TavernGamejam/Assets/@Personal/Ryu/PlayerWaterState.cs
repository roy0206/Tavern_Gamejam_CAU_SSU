using UnityEngine;

public class PlayerWaterState : State<Player>
{
    public override void Enter(Player player)
    {
        player.GetModule<PlayerMovement>().SetWater();
        player.GetModule<Oxygen>().AddChange("Water", 25);
        Debug.Log("Enter Water");
    }

    public override void Execute(Player player)
    {
        
        if (Physics2D.OverlapCircle(player.transform.position, 0.1f, LayerMask.GetMask("Water"))== null)
        {
            player.GetModule<StateMachine<Player>>().ChangeState("Land");
        }
        
    }

    public override void Exit(Player player)
    {
        player.GetModule<Oxygen>().RemoveChange("Water");
    }
}