using System.Collections;
using UnityEngine;

public class Oil : Entity_baseclass
{
    float curtime = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (curtime > 3 && collision.TryGetComponent(out Player player))
        {
            player.GetModule<PlayerMovement>().Slip();
            curtime = 0;
        }
    }
    new protected void Update()
    {
        base.Update();
        curtime += Time.deltaTime;
    }
}
