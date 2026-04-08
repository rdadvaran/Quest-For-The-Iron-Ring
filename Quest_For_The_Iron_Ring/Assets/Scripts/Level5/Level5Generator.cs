using System.Collections.Generic;
using UnityEngine;

public class Level5Generator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject[] characterPrefabs;
    public GameObject enemyPrefab;
    public GameObject doorPrefab;

    public Transform platformSpawnParent;

    private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> spawnedPlatforms = new List<GameObject>();
    private List<GameObject> availablePlatforms = new List<GameObject>();

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<GameObject> spawnedDoors = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in platformSpawnParent)
        {
            spawnPoints.Add(child);
        }

        Shuffle(spawnPoints);

        for (int i = 0; i < 7; i++)
        {
            GameObject platform = Instantiate(platformPrefab, spawnPoints[i].position, Quaternion.identity);
            spawnedPlatforms.Add(platform);
            availablePlatforms.Add(platform);
        }

        SpawnPlayer();
        SpawnDoors();
        SpawnEnemies();
        AssignEnemiesToDoors();
    }

    void SpawnPlayer()
    {
        int index = Random.Range(0, availablePlatforms.Count);

        GameObject platform = availablePlatforms[index];
        Vector3 spawnPos = platform.transform.position + new Vector3(0, 1.5f, 0);

        GameObject selectedPrefab = characterPrefabs[0];

        if (GameManager.Instance != null)
        {
            string character = GameManager.Instance.selectedCharacter;

            foreach (GameObject prefab in characterPrefabs)
            {
                if (prefab.name == character)
                {
                    selectedPrefab = prefab;
                    break;
                }
            }
        }

        GameObject player = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        player.transform.localScale = enemyPrefab.transform.localScale * 1.25f;

        availablePlatforms.RemoveAt(index);
    }

    void SpawnDoors()
    {
        List<GameObject> tempPlatforms = new List<GameObject>(availablePlatforms);
        ShuffleGameObjects(tempPlatforms);

        for (int i = 0; i < 4; i++)
        {
            GameObject platform = tempPlatforms[i];

            Vector3 spawnPos = platform.transform.position + new Vector3(1.5f, 1.2f, 0);

            GameObject door = Instantiate(doorPrefab, spawnPos, Quaternion.identity);

            spawnedDoors.Add(door);
        }
    }

    void SpawnEnemies()
    {
        int enemyCount = 4;

        Dictionary<GameObject, int> platformEnemyCount = new Dictionary<GameObject, int>();

        int spawned = 0;

        while (spawned < enemyCount)
        {
            GameObject platform = availablePlatforms[Random.Range(0, availablePlatforms.Count)];

            if (!platformEnemyCount.ContainsKey(platform))
                platformEnemyCount[platform] = 0;

            if (platformEnemyCount[platform] >= 2)
                continue;

            int count = platformEnemyCount[platform];

            float offsetX = -1.8f + (count * 1.2f);

            Vector3 spawnPos = platform.transform.position + new Vector3(offsetX, 1.5f, 0);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            spawnedEnemies.Add(enemy);

            platformEnemyCount[platform]++;
            spawned++;
        }
    }

    void AssignEnemiesToDoors()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            int index = Random.Range(0, spawnedDoors.Count);

            GameObject assignedDoor = spawnedDoors[index];

            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.assignedDoor = assignedDoor;
            }
        }
    }

    void Shuffle(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);

            Transform temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    void ShuffleGameObjects(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);

            GameObject temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}