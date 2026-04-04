using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceSpawner : MonoBehaviour
{
    public GameObject puzzlePiecePrefab;
    public Transform spawnAreaParent;
    public int numberOfPiecesToSpawn = 6;

    private List<PuzzleSpawnArea> spawnAreas = new List<PuzzleSpawnArea>();

    private void Start()
    {
        foreach (Transform child in spawnAreaParent)
        {
            PuzzleSpawnArea area = child.GetComponent<PuzzleSpawnArea>();
            if (area != null)
            {
                spawnAreas.Add(area);
            }
        }

        SpawnPuzzlePieces();
    }

    private void SpawnPuzzlePieces()
    {
        List<PuzzleSpawnArea> availableAreas = new List<PuzzleSpawnArea>(spawnAreas);

        if (numberOfPiecesToSpawn > availableAreas.Count)
        {
            numberOfPiecesToSpawn = availableAreas.Count;
        }

        for (int i = 0; i < numberOfPiecesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableAreas.Count);
            PuzzleSpawnArea chosenArea = availableAreas[randomIndex];

            Vector2 spawnPosition = chosenArea.GetRandomPosition();

            Instantiate(puzzlePiecePrefab, spawnPosition, Quaternion.identity);

            availableAreas.RemoveAt(randomIndex);
        }
    }
}