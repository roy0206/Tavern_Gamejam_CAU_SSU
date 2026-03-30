using System.Collections;
using UnityEngine;

public class NetCaughtModule : Module
{
    Sprite _caughtSprite;
    float _multiplier;
    float _duration;
    GameObject _overlay;

    public NetCaughtModule(MonoThing thing) : base(thing) { }

    
    
    
    
    public override void Init(params object[] objects)
    {
        _caughtSprite = (Sprite) objects[0];
        _multiplier   = (float) objects[1];
        _duration     = (float) objects[2];
        base.Init();

        CreateOverlay();
        Thing.StartCoroutine(RemoveAfterDuration(_duration));
    }

    void CreateOverlay()
    {
        _overlay = new GameObject("NetCaughtOverlay");
        _overlay.transform.SetParent(Thing.transform);
        _overlay.transform.localPosition = Vector3.zero;
        _overlay.transform.localScale    = Vector3.one * 4.3f;
        SpriteRenderer sr;
        if(!_overlay.TryGetComponent(out sr)) return;
        
        sr.sprite = _caughtSprite;

        if (Thing.SpriteRenderer != null)
        {
            sr.sortingLayerName = Thing.SpriteRenderer.sortingLayerName;
            sr.sortingOrder     = Thing.SpriteRenderer.sortingOrder + 1;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Vector2 vel = Thing.Rigidbody.linearVelocity;
        vel.x *= _multiplier;
        
        Thing.Rigidbody.linearVelocity = vel;
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        if (_overlay != null)
            Object.Destroy(_overlay);
    }

    IEnumerator RemoveAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Thing.RemoveModule<NetCaughtModule>();
    }
}
