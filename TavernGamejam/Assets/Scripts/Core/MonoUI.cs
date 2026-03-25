using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonoUI : MonoBehaviour
{
    private Canvas rootCanvas;
    private Image image;
    private Button button;
    [SerializeField] private TMP_Text linkedText;

    private void Awake()
    {
        rootCanvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }
}
