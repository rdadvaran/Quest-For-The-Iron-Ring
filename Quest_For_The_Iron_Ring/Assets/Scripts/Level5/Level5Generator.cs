using System.Collections.Generic;
using UnityEngine;

public class Level5Generator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject[] characterPrefabs;
    public GameObject enemyPrefab;
    public GameObject doorPrefab;
    public GameObject playerBulletPrefab;

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
        
        Vector3 spawnPos = platform.transform.position + new Vector3(0, 2f, 0);
        
        GameObject selectedPrefab = characterPrefabs[0];
        
        if (GameManager5.Instance != null)
        {
            string character = GameManager5.Instance.selectedCharacter;
            foreach (GameObject prefab in characterPrefabs)
            {
                if (prefab.name.Trim() == character.Trim())
                {
                    selectedPrefab = prefab;
                    break;
                }
            }
        }
        
        GameObject player = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        
        playerMovement oldMovement = player.GetComponent<playerMovement>();
        if (oldMovement != null)
        {
            Destroy(oldMovement);
        }
        
        UnityEngine.InputSystem.PlayerInput input = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if(input != null)
        {
            input.enabled = false;
        }
        
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 3f;
        }
        
        PlayerController controller = player.AddComponent<PlayerController>();
        
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.parent = player.transform;
        groundCheck.transform.localPosition = new Vector3(0, -0.6f, 0);
        controller.groundCheck = groundCheck.transform;
        
        controller.groundLayer = Physics2D.AllLayers;
        
        GameObject firePoint = new GameObject("FirePoint");
        firePoint.transform.parent = player.transform;
        firePoint.transform.localPosition = new Vector3(0.5f, 0.2f, 0);
        
        controller.Initialize(playerBulletPrefab, firePoint.transform);
        
        player.tag = "Player";
        player.transform.localScale = Vector3.one * 0.9f;
        
        availablePlatforms.RemoveAt(index);
    }

    void SpawnDoors()
    {
        List<GameObject> tempPlatforms = new List<GameObject>(availablePlatforms);
        ShuffleGameObjects(tempPlatforms);

        for (int i = 0; i < 4; i++)
        {
            GameObject platform = tempPlatforms[i];

            Vector3 spawnPos = platform.transform.position + new Vector3(1.5f, 1.5f, 0);

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

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SetPlatform(platform);
            }

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