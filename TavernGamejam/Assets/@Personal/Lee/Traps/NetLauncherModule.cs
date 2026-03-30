using System.Collections;
using UnityEngine;

public class NetLauncherModule : Module
{
    NetLauncher _launcher;
    FallingNet _netPrefab;
    float _detectionDistance;
    float _coneHalfAngle;
    float _launchDelay;
    float _netWidthOverride;
    bool _fired = false;

    public NetLauncherModule(MonoThing thing) : base(thing)
    {
        _launcher = (NetLauncher)thing;
    }

    public override void Init(params object[] objects)
    {
        _netPrefab         = (FallingNet)objects[0];
        _detectionDistance = (float)objects[1];
        _coneHalfAngle     = (float)objects[2];
        _launchDelay       = (float)objects[3];
        _netWidthOverride  = (float)objects[4];
        base.Init();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_fired) return;

        if (IsPlayerInCone())
        {
            _fired = true;
            _launcher.StartCoroutine(LaunchAfterDelay());
        }
    }

    bool IsPlayerInCone()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            _launcher.transform.position,
            _detectionDistance);

        foreach (Collider2D col in hits)
        {
            if (!col.TryGetComponent<Player>(out _)) continue;

            Vector2 headPos  = new Vector2(col.bounds.center.x, col.bounds.max.y);
            Vector2 toTarget = headPos - (Vector2)_launcher.transform.position;

            if (toTarget.y >= 0f) continue;

            float angle = Vector2.Angle(Vector2.down, toTarget);
            if (angle <= _coneHalfAngle)
                return true;
        }

        return false;
    }

    IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(_launchDelay);

        float netWidth = _netWidthOverride > 0f
            ? _netWidthOverride
            : 2f * _detectionDistance * Mathf.Tan(_coneHalfAngle * Mathf.Deg2Rad);

        FallingNet net = Object.Instantiate(
            _netPrefab,
            _launcher.transform.position,
            Quaternion.identity);

        net.Init(_detectionDistance, netWidth);
    }
}
