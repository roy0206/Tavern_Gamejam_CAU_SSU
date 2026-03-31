using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.15f;
    public float speed = 8f;

    Vector3 _originalScale;
    Vector3 _targetScale;

    void Start()
    {
        _originalScale = transform.localScale;
        _targetScale = _originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetScale = _originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetScale = _originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * speed);
    }
}