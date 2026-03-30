using UnityEngine;

public class seabomb : Entity_baseclass
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            bomb();
        }
    }
    void bomb()
    {
        eb.deathType = DeathType.bomb;
        eb.player.Dead(eb.deathType);
        Destroy(gameObject);
    }
   
    }
