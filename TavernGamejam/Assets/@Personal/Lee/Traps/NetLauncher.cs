using UnityEngine;

public class NetLauncher : MonoThing
{
    [SerializeField] FallingNet netPrefab;
    [SerializeField] float detectionDistance = 5f;
    [SerializeField] float coneHalfAngleDeg  = 45f;
    [SerializeField] float launchDelay       = 0.1f;
    
    [SerializeField] float netWidthOverride  = 0f;

    new void Awake()
    {
        base.Awake();
        AddModule(new NetLauncherModule(this))
            .Init(netPrefab, detectionDistance, coneHalfAngleDeg, launchDelay, netWidthOverride);
    }
}
