using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject normalBugPrefab;
    [SerializeField] private GameObject fastBugPrefab;

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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 spawnPosition = Vector2.zero;
        bool validPosition = false;

        int attempts = 0;
        float minimumDistanceFromPlayer = 2.5f;

        while (!validPosition && attempts < 20)
        {
            spawnPosition = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            if (player == null)
            {
                validPosition = true;
            }
            else
            {
                float distance = Vector2.Distance(spawnPosition, player.transform.position);
                if (distance >= minimumDistanceFromPlayer)
                {
                    validPosition = true;
                }
            }

            attempts++;
        }

        if (!validPosition)
            return;

        float randomValue = Random.value;
        GameObject bugToSpawn;

        if (randomValue < 0.5f || fastBugPrefab == null)
        {
            bugToSpawn = normalBugPrefab;
        }
        else
        {
            bugToSpawn = fastBugPrefab;
        }

        Instantiate(bugToSpawn, spawnPosition, Quaternion.identity);
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