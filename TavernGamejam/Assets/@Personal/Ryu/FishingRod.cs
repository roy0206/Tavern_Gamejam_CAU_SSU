using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FishingRod : Entity_baseclass
{
    Player player;
    [SerializeField] Transform hanger;
    bool isActivated = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated) return;
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.Dead(DeathType.Stab);
            this.player = player;
            transform.DOMoveY(10, 01f).SetEase(Ease.OutCubic);
            isActivated = true;
        }
    }

    new protected void Update()
    {
        if (!isActivated) return;

        player.transform.position = hanger.transform.position;

    }
}
