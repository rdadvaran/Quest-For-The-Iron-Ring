using UnityEngine;

public class Level3PlayerSetup : MonoBehaviour
{
    [Header("Shape Jam Speed")]
    [SerializeField] private float baseMoveSpeed = 8f;
    [SerializeField] private float boostedMoveSpeed = 14f;

    [Header("Boosted Characters")]
    [SerializeField] private string gamerCharacterName = "Gamer_Player";
    [SerializeField] private string aiCharacterName = "AI_Player";

    [Header("Shape Jam Bounds")]
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -4.275f;
    [SerializeField] private float maxY = 5.125f;

    private bool setupComplete = false;

    private void Update()
    {
        if (setupComplete) return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return;

        // Add Level 3 health/controller only to Shape Jam player
        PlayerController3 playerController3 = playerObject.GetComponent<PlayerController3>();
        if (playerController3 == null)
        {
            playerObject.AddComponent<PlayerController3>();
        }

        // Base speed for everyone, boost only for Gamer or AI
        playerMovement movement = playerObject.GetComponent<playerMovement>();
        if (movement != null)
        {
            float speedToUse = baseMoveSpeed;

            string playerName = playerObject.name.Replace("(Clone)", "").Trim();

            if (playerName == gamerCharacterName || playerName == aiCharacterName)
            {
                speedToUse = boostedMoveSpeed;
            }

            movement.SetMoveSpeed(speedToUse);
            Debug.Log("Shape Jam speed applied to " + playerName + ": " + speedToUse);
        }

        // Add and configure Shape Jam bounds only to this spawned player
        Level3PlayerBounds bounds = playerObject.GetComponent<Level3PlayerBounds>();
        if (bounds == null)
        {
            bounds = playerObject.AddComponent<Level3PlayerBounds>();
        }

        bounds.SetBounds(minX, maxX, minY, maxY);

        setupComplete = true;
    }
}