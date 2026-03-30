using DG.Tweening;
using UnityEngine;

public class MovingObject : Entity_baseclass
{
    [SerializeField] protected Transform point1;
    [SerializeField] protected Transform point2;
    [SerializeField] protected float timePerMove;
    [SerializeField] protected Ease ease;

    private void Start()
    {
        transform.position = point1.position;
        MoveTo2();
    }
    void MoveTo1()
    {
        transform.DOMove(point1.position, timePerMove).SetEase(ease).OnComplete(MoveTo2);
    }
    void MoveTo2()
    {
        transform.DOMove(point2.position, timePerMove).SetEase(ease).OnComplete(MoveTo1);
    }

    public void Init(Transform p1, Transform p2, float tpm, Ease e)
    {
        point1 = p1;
        point2 = p2;
        timePerMove = tpm;
        ease = e;
        DOVirtual.DelayedCall(timePerMove, () => { transform.DOKill();SpriteRenderer.DOFade(0, 0.5f).OnComplete(() => { Destroy(gameObject); });});

    }
}
