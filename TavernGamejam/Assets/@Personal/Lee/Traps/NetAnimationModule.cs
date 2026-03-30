using UnityEngine;

public class NetAnimationModule : Module
{
    Sprite[] _frames;
    float _interval;
    int _currentFrame;
    float _timer;

    public NetAnimationModule(MonoThing thing) : base(thing) { }

    public override void Init(params object[] objects)
    {
        _frames   = (Sprite[])objects[0];
        _interval = (float)objects[1];
        base.Init();

        if (Thing.SpriteRenderer != null && _frames.Length > 0)
            Thing.SpriteRenderer.sprite = _frames[0];
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        _timer += Time.deltaTime;
        if (_timer < _interval) return;

        _timer -= _interval;
        _currentFrame = (_currentFrame + 1) % _frames.Length;

        if (Thing.SpriteRenderer != null)
            Thing.SpriteRenderer.sprite = _frames[_currentFrame];
    }
}
