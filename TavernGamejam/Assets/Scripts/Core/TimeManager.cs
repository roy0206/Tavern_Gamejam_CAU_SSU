using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    [SerializeField] float timeScale;
    public float TImeScale => timeScale;
    public float DeltaTime => Time.deltaTime * timeScale;  
}
