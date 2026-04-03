using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour, ISavable
{
    public Button playButton;
    public Button stopButton;
    public Button continueButton;

    public TMP_Text logoText;
    public TMP_Text cleared;
    
    
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public float speed = 1.5f;
    
    float _originalScale;
    float _originalScale2;
    int a1;
    
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
        _originalScale2 = cleared.fontSize;

        a1 = AudioManager.Instance.PlaySound("StartBGM", transform, 1, 999);

    }

    void Update()
    {
        if (logoText == null) return;


        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f; // 0 ~ 1
        float scale = Mathf.Lerp(minScale, maxScale, t);
        logoText.fontSize = _originalScale * scale;
        cleared.fontSize = _originalScale2 * scale;
    }

    IEnumerator NewGame()
    {
        DataManager.Instance.NewGame();
        yield return new WaitForSeconds(0.3f);

        AudioManager.Instance.StopSound(a1);

        
        SceneController.Instance.LoadScene("Main"); 

    }

    public void LoadData(Database data)
    {
        if (data.isCleared) cleared.gameObject.SetActive(true);
    }

    public void SaveData(ref Database data)
    {

    }
}