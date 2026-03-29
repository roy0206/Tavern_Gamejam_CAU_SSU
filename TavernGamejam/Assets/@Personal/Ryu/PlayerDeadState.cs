using UnityEngine;

public class PlayerDeadState : State<Player>
{
    public override void Enter(Player player)
    {
        player.RemoveModule<PlayerMovement>();
        player.RemoveModule<Oxygen>();
        Debug.Log("PlayerHasDead");
    }

    public override void Execute(Player player)
    {
        
    }

    public override void Exit(Player player)
    {

    }
}