using UnityEngine;

public class seabomb : Entity_baseclass
{
    
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bomb();
        }
    }
    void bomb()
    {
        Destroy(gameObject);
    }
   
    }
