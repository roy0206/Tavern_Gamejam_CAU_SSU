using UnityEngine;

[CreateAssetMenu(fileName = "WaterSettings", menuName = "ScriptableObjects/WaterSettings")]
public class WaterSettings : ScriptableObject
{
    static WaterSettings _currentSettings;
    public static WaterSettings currentSettings =>
        _currentSettings ??= Resources.Load<WaterSettings>("WaterSettings");
    
    [Header("Physics")]
    public float tension;
    public float damping;
    public float spread;
    public int iterationsPerFrame = 1;
    
    [Header("Collision")]
    public float surfaceCollisionDistance;
    public float collisionVelocityTransfer;

    [Header("Optimization")]
    public float simulationDistance;
    public float nodePerUnit;
}