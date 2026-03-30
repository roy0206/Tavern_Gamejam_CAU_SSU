using System.Collections;
using UnityEngine;

public class SpeedDebuffModule : Module
{
    float _multiplier;
    float _duration;

    public SpeedDebuffModule(MonoThing thing) : base(thing) { }

    public override void Init(params object[] objects)
    {
        _multiplier = (float)objects[0];
        _duration   = (float)objects[1];
        base.Init();

        Thing.StartCoroutine(RemoveAfterDuration(_duration));
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Vector2 vel = Thing.Rigidbody.linearVelocity;
        vel.x *= _multiplier;
        Thing.Rigidbody.linearVelocity = vel;
    }

    IEnumerator RemoveAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Thing.RemoveModule<SpeedDebuffModule>();
    }
}
