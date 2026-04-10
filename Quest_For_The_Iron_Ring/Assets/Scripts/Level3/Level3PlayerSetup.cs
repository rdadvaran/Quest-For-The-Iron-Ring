using UnityEngine;

public class Level3PlayerSetup : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float boostedSpeed = 6.5f;

    private bool setupComplete = false;

    private void Update()
    {
        if (setupComplete) return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return;

        // Add health/controller script only to this spawned instance
        PlayerController playerController = playerObject.GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerObject.AddComponent<PlayerController>();
        }

        // Adjust movement speed only for this spawned instance
        playerMovement movement = playerObject.GetComponent<playerMovement>();
        if (movement != null)
        {
            float speedToUse = normalSpeed;

            if (GameSession.Instance != null)
            {
                string selected = GameSession.Instance.selectedCharacter;

                if (selected == "Gamer_Player" || selected == "AI_Player")
                {
                    speedToUse = boostedSpeed;
                }
            }

            movement.SetMoveSpeed(speedToUse);
        }

        setupComplete = true;
    }
}