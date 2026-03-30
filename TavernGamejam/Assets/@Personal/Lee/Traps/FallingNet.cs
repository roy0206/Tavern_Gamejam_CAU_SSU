using UnityEngine;

public class FallingNet : MonoThing
{
    [SerializeField] Sprite caughtSprite;

    FallingNetModule _module;

    new void Awake()
    {
        base.Awake();
    }

    public void Init(float fallDistance, float netWidth)
    {
        _module = new FallingNetModule(this);
        AddModule(_module).Init(fallDistance, netWidth, caughtSprite);
    }

    void OnTriggerEnter2D(Collider2D other) => _module?.OnTriggerEnter(other);
}
