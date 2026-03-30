using System;
using UnityEngine;

public class StartPlayerController : MonoBehaviour
{
    [SerializeField] Player player;

    void Start()
    {
        player.GetModule<StateMachine<Player>>().ChangeState("Water");
        
        
    }

    private void Update()
    {
     
        
    }
}
