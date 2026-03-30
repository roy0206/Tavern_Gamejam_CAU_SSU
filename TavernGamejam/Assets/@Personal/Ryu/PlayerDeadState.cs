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

                break;
            case DeathType.Bitten:
                player.GetModule<BloodEmiter>().Bleed(40, false);
                
                break;
            case DeathType.Stab:
                player.GetModule<BloodEmiter>().Bleed(20, true);
                AudioManager.Instance.PlaySound("Stab", player.transform.root, 1, 1);
                break;
            case DeathType.Ground:
                player.GetModule<BloodEmiter>().Bleed(300, false);
                player.SpriteRenderer.enabled = false;
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