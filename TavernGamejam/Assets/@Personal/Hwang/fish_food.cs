using UnityEngine;

public class fish_food : Entity_baseclass
{

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
        if (collision.CompareTag("Player"))
        {
            if (!collision.TryGetComponent<Player>(out var player)) return;
            player.DashCooltime *= 4 / 5;
        }
    }
}
