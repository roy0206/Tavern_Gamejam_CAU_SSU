using UnityEngine;

public class FallingNet : MonoThing
{
    [SerializeField] Sprite _caughtSprite;

    FallingNetModule _module;

    new void Awake()
    {
        base.Awake();
    }

    public void Init(float fallDistance, float netWidth, 
        Vector2 launchDirection, float launchSpeed, float gravityScale)
    {
        _module = new FallingNetModule(this);
        AddModule(_module).Init(fallDistance, netWidth, _caughtSprite, launchDirection, launchSpeed, gravityScale);
    }

    void OnTriggerEnter2D(Collider2D other) => _module?.OnTriggerEnter(other);
}
