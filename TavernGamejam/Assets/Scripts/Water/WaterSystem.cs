using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;

public class WaterSystem
{
    static WaterSystem _instance;
    protected static WaterSystem instance => _instance ??= new WaterSystem();
    
    List<Water> _waters = new();
    List<WaterBody> _bodies = new();
    WaterNodeMappingData _waterNodeMappingData = new();
    SimulationData _simulationData = new();
    
    WaterSettings settings => WaterSettings.currentSettings;
    public static Vector2 simulationCenter => Camera.main.transform.position;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RuntimeInitializeOnLoad()
    {
        _instance = new WaterSystem();
        PlayerLoopSystem system = new PlayerLoopSystem
        {
            type = typeof(UpdateWaterSystem),
            updateDelegate = () => instance.Update()
        };
        system.RegisterTo<UnityEngine.PlayerLoop.PostLateUpdate>();
        Application.quitting += () => instance._simulationData.Dispose();
    }
    void Register_Internal(Water water) => _waters.Add(water);
    void Deregister_Internal(Water water) => _waters.Remove(water);

    void Update()
    {
        UpdatePhysics();
        
        foreach(var water in _simulationData.waters)
        {
            var nodeRange = _simulationData.mappingData.GetNodeRange(water);
            var waterRange = _simulationData.mappingData.GetWaterRange(water);
            for (int i = nodeRange.start; i < nodeRange.end; i++)
            {
                int waterIndex = i - nodeRange.start + waterRange.start;
                float x = water.IndexToX(waterIndex);
                float y = _simulationData.positions[i] + water.bounds.yMax;
                Debug.DrawLine(new Vector3(x,y-0.1f,0),new Vector3(x,y+0.1f,0));
            }
        }
    }

    bool GetPositions_Internal(Water water, out NativeArray<float> positions, out RangeInt waterRange)
    {
        (positions, waterRange) = (default, default);
        if (!_simulationData.Contains(water)) return false;
        waterRange = _simulationData.mappingData.GetWaterRange(water);
        positions = _simulationData.GetPositions(water);
        return true;
    }

    void Collide_Internal(Water water, WaterBody body)
    {
        if (!_simulationData.Contains(water)) return;
        var positions = _simulationData.GetPositions(water);
        var velocities = _simulationData.GetVelocities(water);
        var waterRange = _simulationData.mappingData.GetWaterRange(water);
        var intersect = waterRange.Intersect(water.InnerIndexRange(body.bounds.min.x, body.bounds.max.x));
        for (int i = intersect.start; i < intersect.end; i++)
        {
            int waterIndex = i - waterRange.start;
            float py = positions[waterIndex];
            float v = velocities[waterIndex];
            Vector2 point = new Vector2(water.IndexToX(i), py + water.bounds.yMax);
            if (Mathf.Abs(py) < settings.surfaceCollisionDistance && body.collider.OverlapPoint(point))
            {
                if (Mathf.Abs(body.rigidbody.linearVelocityY) > Mathf.Abs(v) || body.rigidbody.linearVelocityY * v < 0)
                {
                    velocities[waterIndex] = body.rigidbody.linearVelocityY * settings.collisionVelocityTransfer
                        * (1f - Mathf.Abs(py) / settings.surfaceCollisionDistance);
                }
            }
        }
    }

    void UpdatePhysics()
    {
        _waterNodeMappingData.Clear();
        foreach (var water in _waters) _waterNodeMappingData.Add(water, settings, simulationCenter);
        _simulationData.Update(_waterNodeMappingData);
        
        foreach (var water in _simulationData.waters)
        {
            var velocities = _simulationData.GetVelocities(water);
            var positions = _simulationData.GetPositions(water);
            for (int i = 0; i< settings.iterationsPerFrame; i++) {
                var job = new WaterSimulationJobs.VelocityJob(settings, velocities, positions, Time.deltaTime/settings.iterationsPerFrame);
                var handle = job.Schedule(job.velocities.Length, 8);
                handle.Complete();
                var job2 = new WaterSimulationJobs.PositionJob(velocities, positions, Time.deltaTime/settings.iterationsPerFrame);
                var handle2 = job2.Schedule(job2.velocities.Length, 8);
                handle2.Complete();
            }
        }
    }

    public static void Register(Water water) => instance.Register_Internal(water);    
    public static void Deregister(Water water) => instance.Deregister_Internal(water);
    public static void Collide(Water water, WaterBody body) => instance.Collide_Internal(water, body);
    public static bool GetPositions(Water water, out NativeArray<float> positions, out RangeInt range) 
        => instance.GetPositions_Internal(water, out positions, out range);

    class WaterNodeMappingData
    {
        public int totalNodeCount;
        public List<Water> waters = new();
        public List<RangeInt> nodeRanges = new();
        public List<RangeInt> waterRanges = new();

        public void Add(Water water, WaterSettings settings, Vector2 simulationCenter)
        {
            float r = settings.simulationDistance;
            float nodePerUnit = settings.nodePerUnit;
            if (!water.simulatable) return;

            float y = water.bounds.yMax - simulationCenter.y;
            float sqrt = Mathf.Sqrt(r * r - y * y);
            if (float.IsNaN(sqrt)) return;
            RangeInt range = water.OuterIndexRange(simulationCenter.x - sqrt, simulationCenter.x + sqrt);

            waters.Add(water);
            nodeRanges.Add(new RangeInt(totalNodeCount, range.length));
            waterRanges.Add(range);
            totalNodeCount += range.length;
        }

        public RangeInt GetNodeRange(Water water) => nodeRanges[waters.IndexOf(water)];
        public RangeInt GetWaterRange(Water water) => waterRanges[waters.IndexOf(water)];

        public void CopyFrom(WaterNodeMappingData other)
        {
            totalNodeCount = other.totalNodeCount;
            waters.Clear();
            nodeRanges.Clear();
            waterRanges.Clear();
            waters.AddRange(other.waters);
            nodeRanges.AddRange(other.nodeRanges);
            waterRanges.AddRange(other.waterRanges);
        }

        public void Clear()
        {
            totalNodeCount = 0;
            waters.Clear();
            nodeRanges.Clear();
            waterRanges.Clear();
        }
    }
    class SimulationData : IDisposable
    {
        public NativeArray<float> velocities;
        public NativeArray<float> positions;
        public WaterNodeMappingData mappingData = new();
        public List<Water> waters => mappingData.waters;
        public void Update(WaterNodeMappingData newMappingData)
        {
            NativeArray<float> newVelocities = new (newMappingData.totalNodeCount, Allocator.Persistent);
            NativeArray<float> newPositions = new (newMappingData.totalNodeCount, Allocator.Persistent);

            foreach (var water in newMappingData.waters)
            {
                if (!mappingData.waters.Contains(water)) continue;
                RangeInt nodeRange = mappingData.GetNodeRange(water);
                RangeInt waterRange = mappingData.GetWaterRange(water);
                RangeInt newNodeRange = newMappingData.GetNodeRange(water);
                RangeInt newWaterRange = newMappingData.GetWaterRange(water);
                RangeInt waterIntersect = waterRange.Intersect(newWaterRange);
                if (waterIntersect.length == 0) continue;

                NativeArray<float> source =
                    velocities.GetSubArray(nodeRange.start + waterIntersect.start - waterRange.start, waterIntersect.length);
                NativeArray<float> target =
                    newVelocities.GetSubArray(newNodeRange.start + waterIntersect.start - newWaterRange.start, waterIntersect.length);
                target.CopyFrom(source);
                
                source = positions.GetSubArray(nodeRange.start + waterIntersect.start - waterRange.start, waterIntersect.length);
                target = newPositions.GetSubArray(newNodeRange.start + waterIntersect.start - newWaterRange.start, waterIntersect.length);
                target.CopyFrom(source);
            }

            mappingData.CopyFrom(newMappingData);
            if (velocities.IsCreated) velocities.Dispose();
            if (positions.IsCreated) positions.Dispose();
            velocities = newVelocities;
            positions = newPositions;
        }
        
        public NativeArray<float> GetVelocities(Water water) => velocities.GetSubArray( mappingData.GetNodeRange(water));
        public NativeArray<float> GetPositions(Water water) => positions.GetSubArray( mappingData.GetNodeRange(water));
        
        public bool Contains(Water water) => mappingData.waters.Contains(water);
        public void Dispose()
        {
            if (velocities.IsCreated) velocities.Dispose();
            if (positions.IsCreated) positions.Dispose();
        }
    }
    struct UpdateWaterSystem
    {
    }
}