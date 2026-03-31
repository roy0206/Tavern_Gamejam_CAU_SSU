using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public Button playButton;
    public Button stopButton;
    public Button continueButton;

    public TMP_Text logoText;
    
    
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public float speed = 1.5f;
    
    float _originalScale;
    
    
    public void Start()
    {
        playButton.gameObject.AddComponent<ButtonHover>();
        stopButton.gameObject.AddComponent<ButtonHover>();
        continueButton.gameObject.AddComponent<ButtonHover>();
        
        continueButton.onClick.AddListener(() =>
        {
           SceneController.Instance.LoadScene("Main"); 
        });
        
        stopButton.onClick.AddListener(() =>
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        });

        playButton.onClick.AddListener(() =>
        {
            StartCoroutine(NewGame());
        });
        _originalScale = logoText.fontSize;
    }

    void Update()
    {
        if (logoText == null) return;


        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f; // 0 ~ 1
        float scale = Mathf.Lerp(minScale, maxScale, t);
        logoText.fontSize = _originalScale * scale;

    }

    IEnumerator NewGame()
    {
        DataManager.Instance.NewGame();
        yield return new WaitForSeconds(0.3f);
        
        SceneController.Instance.LoadScene("Main"); 

    }

}