using DG.Tweening;
using UnityEngine;

public class PlayerDeadState : State<Player>
{
    public override void Enter(Player player)
    {
        player.RemoveModule<PlayerMovement>();
        player.RemoveModule<Oxygen>();
        player.RemoveModule<DashIndicator>();
        player.Animator.enabled = false;
        Debug.Log("PlayerHasDead");

        AudioManager.Instance.StopSound(player.bgmId);
        AudioManager.Instance.PlaySound("Dead", player.transform.root, 1, 1);

        switch (player.DeathType)
        {
            case DeathType.Suffocated:
                player.SpriteRenderer.DOColor(new Color(0.2f, 0.2f, 0.2f), 1);
                break;
            case DeathType.Bitten:
                player.GetModule<BloodEmiter>().Bleed(40, false);
                AudioManager.Instance.PlaySound("Bite", player.transform.root, 1, 1);
                break;
            case DeathType.Stab:
                player.GetModule<BloodEmiter>().Bleed(20, true);
                AudioManager.Instance.PlaySound("Stab", player.transform.root, 1, 1);
                break;
            case DeathType.Ground:
                player.GetModule<BloodEmiter>().Bleed(300, false);
                AudioManager.Instance.PlaySound("Stab", player.transform.root, 1, 1);
                player.SpriteRenderer.enabled = false;
                break;
        }
        DOVirtual.DelayedCall(3, () => { SceneController.Instance.LoadScene("Main"); });

    }

    public override void Execute(Player player)
    {
        
    }

    public override void Exit(Player player)
    {

    }
}