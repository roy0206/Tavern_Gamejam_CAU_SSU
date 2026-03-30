using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Entity_baseclass : MonoThing
{
    public Player player;
    public DeathType deathType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }


}
