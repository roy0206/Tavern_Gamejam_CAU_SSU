using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishComponent : MonoBehaviour
{
    public GameObject finishPanel;
    public TMP_Text finishText;
    public Button finishButton;

    float _hue = 0f;
    public float rainbowSpeed = 0.5f;
    
    public float moveSpeed = 300f;

    RectTransform _textRect;
    Vector2 _velocity;
    RectTransform _panelRect;

    void Start()
    {
        finishPanel.SetActive(false);

        _textRect = finishText.GetComponent<RectTransform>();
        _panelRect = finishPanel.GetComponent<RectTransform>();

        float angle = Random.Range(0f, Mathf.PI * 2f);
        _velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * moveSpeed;

        finishButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Start");
        });
    }

    void Update()
    {
        _hue += Time.unscaledDeltaTime * rainbowSpeed;
        if (_hue > 1f)
        {
            _hue -= 1f;
            float angle = Random.Range(0f, Mathf.PI * 2f);
            _velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * moveSpeed;
        }
        finishText.color = Color.HSVToRGB(_hue, 1f, 1f);

        if (!finishPanel.activeSelf) return;
        BounceUpdate();
    }

    void BounceUpdate()
    {
        _textRect.anchoredPosition += _velocity * Time.unscaledDeltaTime;

        Vector2 panelHalf = _panelRect.rect.size / 2f;
        Vector2 textHalf  = _textRect.rect.size / 2f;

        Vector2 pos = _textRect.anchoredPosition;

        if (pos.x + textHalf.x >= panelHalf.x)
        {
            pos.x = panelHalf.x - textHalf.x;
            _velocity.x = -Mathf.Abs(_velocity.x);
        }
        else if (pos.x - textHalf.x <= -panelHalf.x)
        {
            pos.x = -panelHalf.x + textHalf.x;
            _velocity.x = Mathf.Abs(_velocity.x);
        }

        if (pos.y + textHalf.y >= panelHalf.y)
        {
            pos.y = panelHalf.y - textHalf.y;
            _velocity.y = -Mathf.Abs(_velocity.y);
        }
        else if (pos.y - textHalf.y <= -panelHalf.y)
        {
            pos.y = -panelHalf.y + textHalf.y;
            _velocity.y = Mathf.Abs(_velocity.y);
        }

        _textRect.anchoredPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            finishPanel.SetActive(true);
            TimeManager.Instance.ChangeTimeScale(0);
        }
    }
}