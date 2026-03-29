using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Piranha : Entity_baseclass
{
  enum State
    {
        idle,
        idletochase,
        chase,
        chasetotidle
    };
    State state_pira;
    Rigidbody2D rb;
    float MoveTimer;
    float Power = 5f;
    float detectradius = 5f;
    bool isPlayer;
    public bool PlayerBlood;
    Vector2 moveDir;
  
    void Start()
    {
        state_pira = State.idle;
        rb = GetComponent<Rigidbody2D>();
    }
     void Update()
    {
        Collider2D[] detectPlayer = Physics2D.OverlapCircleAll(transform.position, detectradius);
        foreach (var t in detectPlayer) { 
                if(t.tag == "Player")
            {

            }
        }

        switch (state_pira)
        {
            case State.idle:
                idle();
                break;

            case State.idletochase:
                idletochase();
                break;

            case State.chase:
                chase();
                break;
            case State.chasetotidle:
                chasetoidle();
                break;
        }
    }

    void idle()
    {
        rb.linearVelocity *= 0.95f;
        MoveTimer -= Time.deltaTime;
        if (MoveTimer <= 0)
        {
            MoveTimer = Random.Range(0.5f, 1.5f);

            moveDir = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-0.3f, 0.3f)
            ).normalized;
        }
        rb.AddForce(moveDir * (Power * 0.3f));
    }
    void idletochase()
    {
        
    }
    void chase()
    {

    }
    void chasetoidle()
    {

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectradius);
    }
}
