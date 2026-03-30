using UnityEngine;

public class FallingNetModule : Module
{
    FallingNet _net;
    float _fallDistance;
    float _startY;
    Sprite _caughtSprite;
    bool _hasHit = false;
    bool _destroyed = false;

    public FallingNetModule(MonoThing thing) : base(thing)
    {
        _net = (FallingNet)thing;
    }

    public override void Init(params object[] objects)
    {
        _fallDistance  = (float)objects[0];
        float netWidth = (float)objects[1];
        _caughtSprite  = (Sprite)objects[2];

        _startY = _net.transform.position.y;

        Vector3 scale = _net.transform.localScale;
        scale.x = netWidth;
        _net.transform.localScale = scale;

        base.Init();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (_destroyed) return;

        float fallen = _startY - _net.transform.position.y;
        if (fallen >= _fallDistance)
        {
            _destroyed = true;
            Object.Destroy(_net.gameObject);
        }
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (_hasHit) return;
        if (!other.TryGetComponent<Player>(out var player)) return;

        _hasHit = true;

        if (!player.TryGetModule<NetCaughtModule>(out _))
            player.AddModule(new NetCaughtModule(player)).Init(_caughtSprite, 0.5f, 2f);

        _destroyed = true;
        Object.Destroy(_net.gameObject);
    }
}
