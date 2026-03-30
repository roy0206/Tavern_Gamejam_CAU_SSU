using System.Collections;
using UnityEngine;

public class Oil : Entity_baseclass
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.GetModule<PlayerMovement>().Slip();
        }
    }
    new protected void Update()
    {
        base.Update();
    }
}
