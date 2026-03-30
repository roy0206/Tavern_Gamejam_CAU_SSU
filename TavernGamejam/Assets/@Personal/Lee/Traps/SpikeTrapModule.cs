using DG.Tweening;
using UnityEngine;

public class SpikeTrapModule : Module
{
    SpikeTrap _trap;
    bool _hasTriggered = false;

    public SpikeTrapModule(MonoThing thing) : base(thing)
    {
        _trap = (SpikeTrap)thing;
    }

    public override void Init(params object[] objects)
    {
        bool isHidden = (bool)objects[0];

        if (isHidden && _trap.SpriteRenderer != null)
        {
            Color c = _trap.SpriteRenderer.color;
            c.a = 0.15f;
            _trap.SpriteRenderer.color = c;
        }

        base.Init();
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (_hasTriggered) return;
        if (!other.TryGetComponent<Player>(out var player)) return;

        _hasTriggered = true;
        KillPlayer(player);
    }

    void KillPlayer(Player player)
    {
        // player.RemoveModule<PlayerMovement>();
        // player.RemoveModule<StateMachine<Player>>();
        player.Rigidbody.linearVelocity = Vector2.zero;
        player.transform.DORotate(new Vector3(0f, 0f, -90f), 0.5f);
        player.Dead(DeathType.Stab);
    }
}
