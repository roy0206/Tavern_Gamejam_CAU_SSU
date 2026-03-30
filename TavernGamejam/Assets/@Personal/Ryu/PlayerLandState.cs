using UnityEngine;
public class PlayerLandState : State<Player>
{
    public override void Enter(Player player)
    {
        player.GetModule<PlayerMovement>().SetLand();
        player.GetModule<Oxygen>().AddChange("Land", -7.5f);


        player.GetModule<Effector>().RemoveEffect<PlasticBagEffect>();

        Debug.Log("Enter Land");
    }

    public override void Execute(Player player)
    {
        if (Physics2D.OverlapCircle(player.transform.position, 0.1f, LayerMask.GetMask("Water")))
        {
            player.GetModule<StateMachine<Player>>().ChangeState("Water");
        }
        
    }

    public override void Exit(Player player)
    {
        player.GetModule<Oxygen>().RemoveChange("Land");
    }
}