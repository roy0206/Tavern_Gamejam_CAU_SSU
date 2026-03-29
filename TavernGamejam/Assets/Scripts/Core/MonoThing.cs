using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoThing : MonoBehaviour, ISceneEventListener
{
    List<Module> _modules = new();
    public Rigidbody2D Rigidbody => rigidbody;
    new protected Rigidbody2D rigidbody;
    public Collider2D Collider => collider;
    new protected Collider2D collider;
    public Animator Animator => animator;
    protected Animator animator;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    protected SpriteRenderer spriteRenderer;


    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        foreach (var module in _modules) module.OnUpdate();
    }
    protected void LateUpdate()
    {
        foreach (var module in _modules) module.OnLateUpdate();
    }
    protected void FixedUpdate()
    {
        foreach (var module in _modules) module.OnFixedUpdate();
    }

    public Module AddModule(Module newModule)
    {
        newModule.OnAdded();
        _modules.Add(newModule);
        return newModule;
    }
    public T GetModule<T>() where T : Module
    {
        return (T)_modules.Find(module => module is T);
    }
    public bool TryGetModule<T>(out T module) where T : Module
    {
        Module m = (T)_modules.Find(module => module is T);

        if (m == null)
        {
            module = null;
            return false;
        }
        module = (T)m;
        return true;
    }

    public void RemoveModule<T>() where T : Module
    {
        Module m = GetModule<T>();
        m.OnRemoved();
        _modules.Remove(m);
    }

    public void OnSceneLoadStart(string sceneName)
    {
        foreach (var module in _modules) module.OnRemoved();
    }

    public void OnSceneLoadComplete(string sceneName)
    {
    }
}
