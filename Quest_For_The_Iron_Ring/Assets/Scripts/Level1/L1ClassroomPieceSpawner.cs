using System.Collections.Generic;
using UnityEngine;

public class L1ClassroomPieceSpawner : MonoBehaviour
{
    public GameObject puzzlePiecePrefab;
    public Transform spawnAreaParent;

    [HideInInspector]
    public int numberOfPiecesToSpawn = 9;

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
        int difficultyValue = 3; // default = Average Joe

        if (GameSession.Instance != null)
        {
            string selectedDifficulty = GameSession.Instance.selectedDifficulty;
            Debug.Log("Selected difficulty in classroom: " + selectedDifficulty);

            switch (selectedDifficulty)
            {
                case "Idle Slacker":
                    difficultyValue = 2;
                    break;

                case "Average Joe":
                    difficultyValue = 3;
                    break;

                case "Goodie 2 Shoes":
                    difficultyValue = 3;
                    break;

                case "Perfectionist":
                    difficultyValue = 3;
                    break;

                default:
                    difficultyValue = 3;
                    break;
            }
        }

        numberOfPiecesToSpawn = difficultyValue * difficultyValue;

        Debug.Log("Classroom difficulty value: " + difficultyValue);
        Debug.Log("Number of classroom pieces to spawn: " + numberOfPiecesToSpawn);
    }

    private void SpawnPuzzlePieces()
    {
        List<L1ClassroomSpawnArea> availableAreas = new List<L1ClassroomSpawnArea>(spawnAreas);

        if (numberOfPiecesToSpawn > availableAreas.Count)
        {
            numberOfPiecesToSpawn = availableAreas.Count;
            Debug.LogWarning("Not enough spawn areas. Reduced piece count to: " + numberOfPiecesToSpawn);
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