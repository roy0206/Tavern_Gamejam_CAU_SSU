using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module
{
    public MonoThing Thing { get; private set; }

    public Module(MonoThing thing) => Thing = thing;

    bool isInit = false;

    public virtual void Init(params object[] objects)
    {
        isInit = true;
    }

    // Lifecycle
    public virtual void OnFixedUpdate() { if (!isInit) return; }
    public virtual void OnLateUpdate() { if (!isInit) return; }
    public virtual void OnAdded() { if (!isInit) return; }
    public virtual void OnUpdate() { if (!isInit) return; }
    public virtual void OnRemoved() { if (!isInit) return; }


}