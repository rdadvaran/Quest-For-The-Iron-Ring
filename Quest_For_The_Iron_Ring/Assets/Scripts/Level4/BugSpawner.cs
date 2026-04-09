using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject normalBugPrefab;
    [SerializeField] private GameObject fastBugPrefab;
    [SerializeField] private GameObject bigBugPrefab;

    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int maxBugs = 8;

    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -2.5f;
    [SerializeField] private float maxY = 1.5f;

    private float spawnTimer = 0f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        int totalBugCount =
            GameObject.FindGameObjectsWithTag("Bug").Length +
            GameObject.FindGameObjectsWithTag("BigBug").Length;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            if (totalBugCount < maxBugs)
            {
                SpawnBug();
            }
        }
    }

    private void SpawnBug()
    {
        if (!TryGetValidSpawnPosition(2f, out Vector2 spawnPosition))
            return;

        float randomValue = Random.value;
        GameObject bugToSpawn = null;

        if (randomValue < 0.4f)
        {
            bugToSpawn = normalBugPrefab;
        }
        else if (randomValue < 0.8f)
        {
            bugToSpawn = fastBugPrefab;
        }
        else
        {
            bugToSpawn = bigBugPrefab;
        }

        if (bugToSpawn != null)
        {
            Instantiate(bugToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private bool TryGetValidSpawnPosition(float minDistanceFromPlayer, out Vector2 spawnPosition)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        spawnPosition = Vector2.zero;

        for (int i = 0; i < 30; i++)
        {
            Vector2 testPosition = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            if (player == null)
            {
                spawnPosition = testPosition;
                return true;
            }

            float distance = Vector2.Distance(testPosition, player.transform.position);

            if (distance >= minDistanceFromPlayer)
            {
                spawnPosition = testPosition;
                return true;
            }
        }

        return false;
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