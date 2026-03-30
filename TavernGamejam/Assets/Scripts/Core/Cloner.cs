using UnityEngine;

public class Cloner : Singleton<Cloner>
{
    public T Clone<T>(GameObject prefab, Vector3 p, Quaternion q, Transform parent)
    {
        return Instantiate(prefab, p, q, parent).GetComponent<T>();
    }
}
