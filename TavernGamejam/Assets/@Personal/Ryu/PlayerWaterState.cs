using UnityEngine;

public class PlayerWaterState : State<Player>
{
    int swimSound;
    public override void Enter(Player player)
    {
        player.GetModule<PlayerMovement>().SetWater();
        player.GetModule<Oxygen>().AddChange("Water", 50);
        AudioManager.Instance.PlaySound("Diving", player.transform.root, Mathf.Clamp(player.Rigidbody.linearVelocityY / 3, 0, 1), 1);
        swimSound = AudioManager.Instance.PlaySound("Swiming", player.transform.root, 1, 999);
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
        AudioManager.Instance.StopSound(swimSound);
        AudioManager.Instance.PlaySound("Diving", player.transform.root, Mathf.Clamp(Mathf.Abs(player.Rigidbody.linearVelocityY) / 3, 0, 1), 1);
    }
}