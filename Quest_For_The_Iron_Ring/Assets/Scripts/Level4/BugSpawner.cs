using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject normalBugPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxBugs = 5;

    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -2.5f;
    [SerializeField] private float maxY = 1.5f;

    private float spawnTimer = 0f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            if (GameObject.FindGameObjectsWithTag("Bug").Length < maxBugs)
            {
                SpawnBug();
            }
        }
    }

    private void SpawnBug()
    {
        Vector2 randomPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        Instantiate(normalBugPrefab, randomPosition, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

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