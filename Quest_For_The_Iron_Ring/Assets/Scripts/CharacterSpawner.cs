using UnityEngine;

public class LevelCharacterSpawner : MonoBehaviour
{
    public GameObject basicPrefab;
    public GameObject aiPrefab;
    public GameObject artistPrefab;
    public GameObject gamerPrefab;
    public GameObject gymRatPrefab;
    public GameObject hackerPrefab;
    public GameObject jockPrefab;

    public Transform spawnPoint;

    private void Start()
    {
        SpawnSelectedCharacter();
    }

    private void SpawnSelectedCharacter()
    {
        GameObject prefabToSpawn = basicPrefab;

        if (GameSession.Instance != null)
        {
            switch (GameSession.Instance.selectedCharacter)
            {
                case "Player":
                    prefabToSpawn = basicPrefab;
                    break;
                case "AI_Player":
                    prefabToSpawn = aiPrefab;
                    break;
                case "Artist_Player":
                    prefabToSpawn = artistPrefab;
                    break;
                case "Gamer_Player":
                    prefabToSpawn = gamerPrefab;
                    break;
                case "GymRat_Player":
                    prefabToSpawn = gymRatPrefab;
                    break;
                case "Hacker_Player":
                    prefabToSpawn = hackerPrefab;
                    break;
                case "Jock_Player":
                    prefabToSpawn = jockPrefab;
                    break;
                default:
                    prefabToSpawn = basicPrefab;
                    break;
            }
        }

        GameObject spawnedPlayer = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // Tag the spawned player so other scripts can find it
        spawnedPlayer.tag = "Player";
    }
}
    
