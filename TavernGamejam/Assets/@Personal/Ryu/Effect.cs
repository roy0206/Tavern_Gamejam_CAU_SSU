using UnityEngine;

public abstract class Effect
{

    public float MaxDuration => maxDuration;

    protected float maxDuration;

    public float duration;

    protected Effector effector;


    public Effect(Effector effector)
    {
        this.effector = effector;
    }
    public virtual void StartEffect()
    {
        InitEffect();
    }
    public virtual void UpdateEffect()
    {
    }
    public virtual void FixedUpdateEffect()
    {
    }
    public virtual void EndEffect() { }

    public void InitEffect()
    {
        duration = maxDuration;
    }
}
