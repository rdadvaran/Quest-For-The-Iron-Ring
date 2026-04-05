using System.Collections.Generic;
using UnityEngine;

public class Level5Generator : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform platformSpawnParent;

    public int platformCount = 7;

    void Start()
    {
        GeneratePlatforms();
    }

    void GeneratePlatforms()
    {
        List<Transform> spawnPoints = new List<Transform>();

        foreach (Transform child in platformSpawnParent)
        {
            spawnPoints.Add(child);
        }

        Shuffle(spawnPoints);

        for (int i = 0; i < platformCount; i++)
        {
            Instantiate(platformPrefab, spawnPoints[i].position, Quaternion.identity);
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
}