using UnityEngine;

public class StartBubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float spawnInterval = 0.8f;
    public float spawnRangeX = 5f;

    float _timer;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            
            
            float randomX = transform.position.x + Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPos = new Vector3(randomX, transform.position.y, transform.position.z);
            
            Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
        }
    }
}