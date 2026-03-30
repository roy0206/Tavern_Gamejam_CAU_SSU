using UnityEngine;

public class Bubble : MovingObject
{
    [SerializeField] Animation anim;
    [SerializeField] Bubble prefab;
    [SerializeField] float regenTime;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            player.GetModule<Oxygen>().AddChange("Bubble", 15);
        }
        if(collision.gameObject.layer == 12)
        {
/*            anim.Play();*/
            Collider.enabled = false;
            Invoke("Regen", regenTime);
        }
    }
    void Regen()
    {
        Bubble b = Instantiate(prefab, transform.parent);
        b.Init(point1, point2, timePerMove, ease);
        b.anim = anim;
        b.prefab = prefab;
        b.regenTime = regenTime;
        b.Collider.enabled = true;
        Destroy(gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.GetModule<Oxygen>().RemoveChange("Bubble");
        }
    }
}
