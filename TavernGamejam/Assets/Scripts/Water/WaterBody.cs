using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WaterBody : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidbody;
    [HideInInspector]
    public Collider2D collider;

    public Bounds bounds => collider.bounds;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Water water))
        {
            WaterSystem.Collide(water, this);
        }
    }
}