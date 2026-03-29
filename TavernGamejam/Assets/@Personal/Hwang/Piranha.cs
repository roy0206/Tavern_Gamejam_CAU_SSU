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
    Transform Playertransform;
    bool isPlayer;
    public bool PlayerBlood;
    Vector2 moveDir;
    float MaxSpeed = 5.5f;
    void Start()
    {
        state_pira = State.idle;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Collider2D[] detectPlayer = Physics2D.OverlapCircleAll(transform.position, detectradius);

        isPlayer = false;

        foreach (var t in detectPlayer)
        {
            if (t.CompareTag("Player"))
            {
                isPlayer = true;
                Playertransform = t.transform;
                break;
            }
        }
        if (isPlayer)
        {
            if (state_pira == State.idle)
                state_pira = State.idletochase;
        }
        else
        {
            if (state_pira == State.chase)
                state_pira = State.chasetotidle;
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
        rb.linearVelocity = Vector2.zero;
        state_pira = State.chase;
        detectradius = 10f;
    }
    void chase()
    {
        if (Playertransform == null) return;
        Vector2 toPlayer = (Playertransform.position - transform.position).normalized;
        rb.AddForce(toPlayer * Power);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, MaxSpeed);
    }
    void chasetoidle()
    {
        Playertransform = null;
        rb.linearVelocity = Vector2.zero;
        state_pira = State.idle;
        detectradius = 5f;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectradius);
    }
}
