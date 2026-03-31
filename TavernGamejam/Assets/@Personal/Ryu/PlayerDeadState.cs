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

        switch (player.DeathType)
        {
            case DeathType.Suffocated:
                player.Rigidbody.freezeRotation = false;
                player.Rigidbody.linearVelocity = Vector2.zero;
                player.SpriteRenderer.DOColor(new Color(0.2f, 0.2f, 0.2f), 1f);
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
                player.SpriteRenderer.enabled = false;
                AudioManager.Instance.PlaySound("Stab", player.transform.root, 1, 1);
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