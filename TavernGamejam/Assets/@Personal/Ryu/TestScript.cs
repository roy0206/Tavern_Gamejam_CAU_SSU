using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UserInput.Instance.BindKeyDown(KeyCode.Space, () => { SceneController.Instance.LoadScene("Scene1"); });
    }

    // Update is called once per frame
    private void OnDisable()
    {
        UserInput.Instance.UnbindKeyDown(KeyCode.Space, () => { SceneController.Instance.LoadScene("Scene1"); });
    }
}
