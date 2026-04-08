using System.Collections.Generic;
using UnityEngine;

public class L1ClassroomPieceSpawner : MonoBehaviour
{
    public GameObject puzzlePiecePrefab;
    public Transform spawnAreaParent;

    [Header("Difficulty Piece Counts")]
    public int idleSlackerPieces = 4;
    public int averageJoePieces = 6;
    public int goodie2ShoesPieces = 8;
    public int perfectionistPieces = 10;

    [HideInInspector]
    public int numberOfPiecesToSpawn = 6;

    private List<L1ClassroomSpawnArea> spawnAreas = new List<L1ClassroomSpawnArea>();

    private void Start()
    {
        SetPieceCountFromDifficulty();

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

    private void SetPieceCountFromDifficulty()
    {
        numberOfPiecesToSpawn = averageJoePieces;

        if (GameSession.Instance != null)
        {
            string difficulty = GameSession.Instance.selectedDifficulty;
            Debug.Log("Selected difficulty in classroom: " + difficulty);

            switch (difficulty)
            {
                case "Idle Slacker":
                    numberOfPiecesToSpawn = idleSlackerPieces;
                    break;

                case "Average Joe":
                    numberOfPiecesToSpawn = averageJoePieces;
                    break;

                case "Goodie 2 Shoes":
                    numberOfPiecesToSpawn = goodie2ShoesPieces;
                    break;

                case "Perfectionist":
                    numberOfPiecesToSpawn = perfectionistPieces;
                    break;

                default:
                    numberOfPiecesToSpawn = averageJoePieces;
                    break;
            }
        }

        Debug.Log("Number of classroom pieces to spawn: " + numberOfPiecesToSpawn);
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

    public int GetPieceCount()
    {
        return numberOfPiecesToSpawn;
    }
}