using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    [SerializeField] static float timeScale = 1;
    public static float TImeScale => timeScale;
    public static float DeltaTime => Time.deltaTime * timeScale;  
    public static float FixedDeltaTime => Time.fixedDeltaTime * timeScale;  

    public void ChangeTimeScale(float scale)
    {
        timeScale = scale;
    }
}
