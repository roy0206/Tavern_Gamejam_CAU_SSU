using UnityEngine;

public class Bubble : MovingObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            player.GetModule<Oxygen>().AddChange("Bubble", 25);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.GetModule<Oxygen>().RemoveChange("Bubble");
        }
    }
}
