using UnityEngine;

public class fish_food : Entity_baseclass
{

    Entity_baseclass eb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            eb.player.DashCooltime *= 4 / 5;
        }
    }
}
