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
    Vector2 _launchDirection;
    float _launchSpeed;
    float _gravityScale;
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
        _launchDirection   = ((Vector2)objects[5]).normalized;
        _launchSpeed       = (float)objects[6];
        _gravityScale      = (float)objects[7];
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

            if (Vector2.Dot(toTarget, _launchDirection) <= 0f) continue;

            float angle = Vector2.Angle(_launchDirection, toTarget);
            if (angle <= _coneHalfAngle)
            {
                
                return true;
            }
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

        // Debug.Log("check");
        net.Init(_detectionDistance, netWidth, _launchDirection, _launchSpeed, _gravityScale);
    }
}
