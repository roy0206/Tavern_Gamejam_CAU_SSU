using UnityEngine;
using DG.Tweening;

public class BubbleSpawner : MonoThing
{
    [SerializeField] Bubble prefab;
    [SerializeField] Transform p1;
    [SerializeField] Transform p2;
    [SerializeField] float timePerMove;
    [SerializeField] Ease ease;
    [SerializeField] float coolTime;

    private void Start()
    {
        curTime = coolTime;
    }
    float curTime;
    new protected void Update()
    {
        base.Update();

        curTime += TimeManager.DeltaTime;
        if(curTime >= coolTime)
        {
            curTime -= coolTime;
            Spawn();
        }
    }

    void Spawn()
    {
        Bubble b = Instantiate(prefab, p1.position, Quaternion.identity);
        b.Init(p1, p2, timePerMove, ease);
    }

}
