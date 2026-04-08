using UnityEngine;

public class CoffeeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coffeePrefab;
    [SerializeField] private float spawnInterval = 40f;
    [SerializeField] private float coffeeLifetime = 5f;

    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    private float spawnTimer = 0f;
    private GameObject currentCoffee;

    private void Update()
    {
        if (currentCoffee != null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnCoffee();
            spawnTimer = 0f;
        }
    }

    private void SpawnCoffee()
    {
        Vector2 randomPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        currentCoffee = Instantiate(coffeePrefab, randomPosition, Quaternion.identity);
        Destroy(currentCoffee, coffeeLifetime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = new Vector3(
            (minX + maxX) / 2f,
            (minY + maxY) / 2f,
            0f
        );

        Vector3 size = new Vector3(
            maxX - minX,
            maxY - minY,
            0f
        );

        Gizmos.DrawWireCube(center, size);
    }
}