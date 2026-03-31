using UnityEngine;

public class StartBubble : MonoBehaviour
{
    
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float destroyHeight = 20f;

    
    public float wobbleAmount = 0.3f;
    public float wobbleSpeed = 2f;

    public float speed;
    public float wobbleOffset; 
    public float startX;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        wobbleOffset = Random.Range(0f, Mathf.PI * 2f);
        startX = transform.position.x;
    }

    void Update()
    {
        transform.position += Vector3.up *( speed * TimeManager.DeltaTime);

        
        float wobbleX = Mathf.Sin(Time.time * wobbleSpeed + wobbleOffset) * wobbleAmount;
        
        transform.position = new Vector3(
            startX + wobbleX,
            
            
            transform.position.y,
            transform.position.z
        );

        if (transform.position.y >= destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}