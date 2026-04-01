using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

class StartSceneController : MonoThing
{
    int id;
    private void Start()
    {
        id = AudioManager.Instance.PlaySound("StartBGM", transform, 1, 999);
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 359));
        Rigidbody.AddTorque(UnityEngine.Random.Range(-3f, 3f));
    }

    private void OnDestroy()
    {
        AudioManager.Instance.StopSound(id);
    }
}
