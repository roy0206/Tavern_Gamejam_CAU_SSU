using UnityEngine;

public class SpikeTrap : MonoThing
{
    [SerializeField] bool _isHidden = false;

    SpikeTrapModule _module;

    new void Awake()
    {
        base.Awake();
        _module = new SpikeTrapModule(this);
        AddModule(_module).Init(_isHidden);
    }

    void OnTriggerEnter2D(Collider2D other) => _module.OnTriggerEnter(other);
}
