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

    public bool PlayerBlood = false;

    Vector2 moveDir;

    float MaxSpeed = 5.5f;

    Entity_baseclass eb;
    GameObject playerobj;

    public LayerMask waterLayer;

    void Start()
    {
        state_pira = State.idle;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        bool isInWater = Physics2D.OverlapPoint(transform.position, waterLayer);

        if (!isInWater)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        if (PlayerBlood == false)
        {
            Collider2D[] detectPlayer = Physics2D.OverlapCircleAll(transform.position, detectradius);
            isPlayer = false;

            foreach (var t in detectPlayer)
            {
                if (t.CompareTag("Player"))
                {

                    if (Physics2D.OverlapPoint(t.transform.position, waterLayer))
                    {
                        isPlayer = true;
                        Playertransform = t.transform;
                        break;
                    }
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
        else
        {
            if (playerobj == null) return;

            Vector2 toPlayer = (playerobj.transform.position - transform.position).normalized;
            rb.AddForce(toPlayer * Power);
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, MaxSpeed);
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

<<<<<<< Updated upstream
        rb.AddForce(toPlayer * Power);
=======
        sr.flipX = dir.x < 0;

        transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        
        rb.AddForce(dir * Power);
>>>>>>> Stashed changes
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Random.Range(0, 2) == 0 && PlayerBlood == false)
            {
                if (!other.TryGetComponent<Player>(out var player)) return;
                player.Dead(DeathType.Bitten);
            }
            else if (Random.Range(0, 2) == 0 && PlayerBlood == true)
            {
                if (!other.TryGetComponent<Player>(out var player)) return;
                player.Dead(DeathType.Bitten);
            }
            else
            {
                playerobj = other.gameObject;
                PlayerBlood = true;
            }
        }
    }
}