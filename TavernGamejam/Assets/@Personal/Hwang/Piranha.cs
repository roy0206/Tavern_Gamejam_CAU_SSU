using UnityEngine;

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

    SpriteRenderer sr;
    float MoveTimer;
    float Power = 8f;
    float detectradius = 5f;

    Transform Playertransform;
    bool isPlayer;

    public bool PlayerBlood = false;

    Vector2 moveDir;

    public float MaxSpeed = 5.5f;

    GameObject playerobj;

    [SerializeField] float waterTop;
    [SerializeField] float waterBottom;
    [SerializeField] float waterLeft;
    [SerializeField] float waterRight;

    bool isInWater = false;

    void Start()
    {
        state_pira = State.idle;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        isInWater = IsInWater(transform.position);

        if (isInWater)
        {
            rb.gravityScale = 0;
            rb.linearDamping = 3f;
        }
        else
        {
            rb.gravityScale = 10;
            rb.linearDamping = 0f;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!PlayerBlood)
        {
            Collider2D[] detectPlayer = Physics2D.OverlapCircleAll(transform.position, detectradius);
            isPlayer = false;

            foreach (var t in detectPlayer)
            {
                if (t.CompareTag("Player"))
                {
                    if (IsInWater(t.transform.position))
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

    void FixedUpdate()
    {
        if (isInWater)
        {
            ClampToWater();


            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                Mathf.Clamp(rb.linearVelocity.y, -2f, 2f)
            );
        }
    }

    bool IsInWater(Vector2 pos)
    {
        return pos.x > waterLeft &&
               pos.x < waterRight &&
               pos.y < waterTop &&
               pos.y > waterBottom;
    }


    void ClampToWater()
    {
        Vector2 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, waterLeft, waterRight);
        pos.y = Mathf.Clamp(pos.y, waterBottom, waterTop);

        transform.position = pos;
    }

    void idle()
    {
        rb.linearVelocity *= 0.95f;

        MoveTimer -= Time.deltaTime;

        if (MoveTimer <= 0)
        {
            MoveTimer = Random.Range(0.5f, 1.5f);

            moveDir = new Vector2(
                Random.Range(-3f, 3f),
                Random.Range(-0.2f, 0.3f)
            ).normalized;
        }

        if (HitWall(moveDir))
        {
            moveDir = -moveDir;
        }
        if (moveDir.x != 0)
            sr.flipX = moveDir.x > 0;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.identity,
            Time.deltaTime * 2f
        );
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

        Vector2 dir = (Playertransform.position - transform.position).normalized;

        sr.flipX = Playertransform.position.x > transform.position.x;
        float angle = dir.y * 30f;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            Time.deltaTime * 5f
        );
        rb.AddForce(dir * Power);
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
        Gizmos.color = Color.blue;
        Vector3 center = new Vector3(
            (waterLeft + waterRight) / 2,
            (waterTop + waterBottom) / 2,
            0
        );

        Vector3 size = new Vector3(
            waterRight - waterLeft,
            waterTop - waterBottom,
            0
        );

        Gizmos.DrawWireCube(center, size);
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
    bool HitWall(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            1f,
            LayerMask.GetMask("Floor"));

        return hit.collider != null;
    }

    void LookAtDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.001f) return;

        sr.flipX = dir.x > 0;

        float angle = dir.y * 30f;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            Time.deltaTime * 5f
        );
    }
}