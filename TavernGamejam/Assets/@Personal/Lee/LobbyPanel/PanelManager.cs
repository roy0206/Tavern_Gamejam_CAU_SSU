using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public Button goToLobbyButton;
    public Button continueButton;
    public GameObject panel;

    void Start()
    {
        goToLobbyButton.onClick.AddListener(() =>
        {
            SceneController.Instance.LoadScene("Start");
        });

        continueButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            TimeManager.Instance.ChangeTimeScale(1);
        });

        UserInput.Instance.BindKeyDown(KeyCode.Escape, TogglePanel);
    }

    void OnDestroy()
    {
        UserInput.Instance.UnbindKeyDown(KeyCode.Escape, TogglePanel);
    }

    void TogglePanel()
    {

        if (panel.activeSelf)
        {
            panel.SetActive(false);
            TimeManager.Instance.ChangeTimeScale(1);
            
        }
        else
        {
            panel.SetActive(true);
            TimeManager.Instance.ChangeTimeScale(0);
            
        }
    }

}