using DG.Tweening;
using UnityEngine;

public class MovingObject : Entity_baseclass
{
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    [SerializeField] float timePerMove;
    [SerializeField] Ease ease;

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
}
