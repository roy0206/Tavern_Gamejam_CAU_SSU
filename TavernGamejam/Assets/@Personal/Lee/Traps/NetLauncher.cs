using UnityEngine;

public class NetLauncher : MonoThing
{
    [SerializeField] FallingNet netPrefab;
    
    [SerializeField] float detectionDistance = 5f;
    [SerializeField] float coneHalfAngleDeg  = 45f;
    [SerializeField] float launchDelay       = 0.1f;
    
    [SerializeField, Range(0f, 360f)] float launchAngleDeg = 270f;
    [SerializeField] float launchSpeed       = 8f;
    [SerializeField, Range(0f, 5f)] float gravityScale = 1f;
    
    [SerializeField] float netWidthOverride  = 0f;

    Vector2 LaunchDir => new Vector2(Mathf.Cos(launchAngleDeg * Mathf.Deg2Rad),
        Mathf.Sin(launchAngleDeg * Mathf.Deg2Rad));

    new void Awake()
    {
        base.Awake();
        AddModule(new NetLauncherModule(this))
            .Init(netPrefab, detectionDistance,
                coneHalfAngleDeg, launchDelay,
                  netWidthOverride, LaunchDir,
                launchSpeed, gravityScale);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        Vector2 dir = LaunchDir;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.6f);
        
        Vector3 leftEdge  = pos + (Vector3)(Rotate(dir,  coneHalfAngleDeg) * detectionDistance);
        Vector3 rightEdge = pos + (Vector3)(Rotate(dir, -coneHalfAngleDeg) * detectionDistance);
        
        Gizmos.DrawLine(pos, leftEdge);
        Gizmos.DrawLine(pos, rightEdge);

        
        
        for (int i = 0; i <24; i++)
        {
            float a = Mathf.Lerp(-coneHalfAngleDeg, coneHalfAngleDeg, (float)i/ 24);
            float b = Mathf.Lerp(-coneHalfAngleDeg, coneHalfAngleDeg, (float)(i +1)/24);
            Gizmos.DrawLine(
                pos + (Vector3)(Rotate(dir, a) * detectionDistance),
                pos + (Vector3)(Rotate(dir, b) * detectionDistance));
        }

        Gizmos.color = Color.yellow;
        
        Vector3 tip = pos + (Vector3)(dir * detectionDistance);
        
        Gizmos.DrawLine(pos, tip);
        Gizmos.DrawLine(tip, tip + (Vector3)(Rotate(-dir,  35f) * 0.6f));
        Gizmos.DrawLine(tip, tip + (Vector3)(Rotate(-dir, -35f) * 0.6f));
        
    }

    
    static Vector2 Rotate(Vector2 v, float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad), s = Mathf.Sin(rad);
        return new Vector2(v.x * c - v.y * s,
                            v.x * s + v.y * c);
    }
}
