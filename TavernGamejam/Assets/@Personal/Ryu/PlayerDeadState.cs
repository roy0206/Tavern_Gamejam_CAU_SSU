using UnityEngine;

public class PlayerDeadState : State<Player>
{
    public override void Enter(Player player)
    {
        player.RemoveModule<PlayerMovement>();
        player.RemoveModule<Oxygen>();
        Debug.Log("PlayerHasDead");

        switch (player.DeathType)
        {
            case DeathType.Suffocated:
                player.Rigidbody.freezeRotation = false;
                break;
            case DeathType.Bitten:
                //Blood
                //SplitBody
                break;
            case DeathType.Stab:
                //Blood
                break;
            case DeathType.Ground:
                //LotOfBlood
                //BodyDisapear
                break;
        }
    }

    public override void Execute(Player player)
    {
        
    }

    public override void Exit(Player player)
    {

    }
}