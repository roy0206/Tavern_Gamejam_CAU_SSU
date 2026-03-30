using System;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    static readonly int k_HeadTint = Shader.PropertyToID("_HeadTint");
    MaterialPropertyBlock _mpb;
    Renderer _renderer;

    public Color headTint = Color.white;

    void Awake()
    {
        _mpb ??= new();
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMpb();
    }

    void UpdateMpb()
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(k_HeadTint, headTint);
        _renderer.SetPropertyBlock(_mpb);
    }
    void OnValidate()
    {
        _mpb ??= new();
        _renderer = GetComponent<Renderer>();
        UpdateMpb();
    }
}
