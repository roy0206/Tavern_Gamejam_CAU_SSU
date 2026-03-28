using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Piranha : Entity_baseclass
{
    Collider2D[] detectRange;
    float MaxSpeed = 200f;
    float Power = 20f;
    float Radius = 5f;
    float moveTimer;
    bool FindPlayer = false;

    Vector2 moveDir;
    Vector2 PiranhatoPlayerDir;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        detectRange = Physics2D.OverlapCircleAll(transform.position, Radius);
        foreach (var t in detectRange)
        {
            if (t.tag == "Player") chase(t.transform);
            else idle();
        }
    }
    void chase(Transform Player)
    {
        Vector2 toPlayer = (Player.position - transform.position).normalized;
        Vector2 currentDir = rb.linearVelocity.normalized;
        Vector2 steeringDir = Vector2.Lerp(currentDir, toPlayer, 0.1f);
        rb.AddForce(steeringDir * Power);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, MaxSpeed);

    }

    void idle()
    {
        rb.linearVelocity *= 0.98f;
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            moveTimer = Random.Range(0.25f, 0.75f);
            moveDir = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-0.05f, 0.05f)
            ).normalized;
        }
        rb.AddForce(moveDir * (Power * 0.3f));
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, Power * 0.4f);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
