using DG.Tweening;
using UnityEngine;

public class FallingNetModule : Module
{
    FallingNet _net;
    float _fallDistance;
    Vector2 _startPos;
    Vector2 _launchDirection;
    float _launchSpeed;
    float _gravityScale;
    Sprite _caughtSprite;
    bool _hasHit = false;
    bool _destroyed = false;

    public FallingNetModule(MonoThing thing) : base(thing)
    {
        _net = (FallingNet)thing;
    }

    public override void Init(params object[] objects)
    {
        _fallDistance    = (float)objects[0];
        float netWidth   = (float)objects[1];
        _caughtSprite    = (Sprite)objects[2];
        _launchDirection = ((Vector2)objects[3]).normalized;
        _launchSpeed     = (float)objects[4];
        _gravityScale    = (float)objects[5];

        // 그냥 크기 적당한거로 하자
        Vector3 targetScale = new Vector3(3.5f, 3.5f, _net.transform.localScale.z);
        _net.transform.localScale = targetScale * 0.1f;

        float angle = Mathf.Atan2(_launchDirection.y, _launchDirection.x) * Mathf.Rad2Deg + 90f;
        _net.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        _net.Rigidbody.isKinematic  = true;
        _net.Rigidbody.gravityScale = _gravityScale;
        _net.Rigidbody.constraints  = RigidbodyConstraints2D.FreezeRotation;

        // 이거 값 때문에 터지던 거였음.
        _startPos = _net.transform.position;

        _net.transform.DOScale(targetScale, 0.4f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _net.Rigidbody.isKinematic    = false;
                _net.Rigidbody.linearVelocity = _launchDirection * _launchSpeed;
            });

        base.Init();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (_destroyed) return;
        
        // 거리까지는 가게 만들려고
        if (Vector2.Distance(_net.transform.position, _startPos) > _fallDistance) {
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
