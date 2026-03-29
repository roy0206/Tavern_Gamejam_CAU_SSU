using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); // A,D / ← →
        float y = Input.GetAxis("Vertical");   // W,S / ↑ ↓

        Vector2 dir = new Vector2(x, y);

        rb.linearVelocity = dir * speed;
    }
}
