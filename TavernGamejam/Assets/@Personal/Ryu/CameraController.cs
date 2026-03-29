using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCam;
    Player plr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
        plr = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetPos = Vector3.Lerp(mainCam.transform.position, plr.transform.position, TimeManager.DeltaTime * 3);
        Vector3 targetPos = plr.transform.position;
        targetPos.z = -10;
        mainCam.transform.position = targetPos;
    }
}
