using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoThing : MonoBehaviour
{
    List<Module> _modules;
    new protected Rigidbody2D rigidbody;
    new protected Collider2D collider;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        foreach (var module in _modules) module.OnUpdate();
    }
    private void LateUpdate()
    {
        foreach (var module in _modules) module.LateUpdate();
    }

    public Module AddModule<T>() where T : Module
    {
        if (GetModule<T>() != null) return null;
        T newModule = Activator.CreateInstance<T>();
        newModule.OnAdded();
        _modules.Add(newModule);
        return newModule;
    }
    public Module GetModule<T>() where T : Module
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
}
