using System.Collections.Generic;
using UnityEngine;

public class L1ClassroomPieceSpawner : MonoBehaviour
{
    public GameObject puzzlePiecePrefab;
    public Transform spawnAreaParent;
    public int numberOfPiecesToSpawn = 6;

    private List<L1ClassroomSpawnArea> spawnAreas = new List<L1ClassroomSpawnArea>();

    private void Start()
    {
        foreach (Transform child in spawnAreaParent)
        {
            L1ClassroomSpawnArea area = child.GetComponent<L1ClassroomSpawnArea>();
            if (area != null)
            {
                spawnAreas.Add(area);
            }
        }

        SpawnPuzzlePieces();
    }

    private void SpawnPuzzlePieces()
    {
        List<L1ClassroomSpawnArea> availableAreas = new List<L1ClassroomSpawnArea>(spawnAreas);

        if (numberOfPiecesToSpawn > availableAreas.Count)
        {
            numberOfPiecesToSpawn = availableAreas.Count;
        }

        for (int i = 0; i < numberOfPiecesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableAreas.Count);
            L1ClassroomSpawnArea chosenArea = availableAreas[randomIndex];

            Vector2 spawnPosition = chosenArea.GetRandomPosition();

            Instantiate(puzzlePiecePrefab, spawnPosition, Quaternion.identity);

            availableAreas.RemoveAt(randomIndex);
        }
    }
}