using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Task3 : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private float levelTime = 180f;
    [SerializeField] private string hubSceneName = "Hub";
    [SerializeField] private int maxIconsForGrade = 20;

    [Header("Phase Times")]
    [SerializeField] private float phase2StartTime = 60f;
    [SerializeField] private float phase3StartTime = 120f;

    [Header("References")]
    [SerializeField] private ProjectileSpawner projectileSpawner;

    [Header("Optional UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text iconText;
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text resultText;

    private float currentTime;
    private bool levelEnded = false;
    private int currentPhase = 1;
    private int iconsCollected = 0;

    private PlayerController player;

    public int Phase => currentPhase;

    private void Start()
    {
        currentTime = levelTime;

        // Try to find the spawned player when level starts
        FindPlayer();

        if (projectileSpawner != null)
        {
            projectileSpawner.UpdateSpawnTable(currentPhase);
        }

        UpdateUI();
    }

    private void Update()
    {
        if (levelEnded) return;

        // If player was not found yet, keep trying
        if (player == null)
        {
            FindPlayer();
        }

        currentTime -= Time.deltaTime;
        if (currentTime < 0f) currentTime = 0f;

        UpdatePhase();
        UpdateUI();

        if (currentTime <= 0f)
        {
            WinLevel();
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
    }


    public void UpdatePhase()
    {
        float elapsed = levelTime - currentTime;
        int newPhase = 1;

        if (elapsed >= phase3StartTime)
            newPhase = 3;
        else if (elapsed >= phase2StartTime)
            newPhase = 2;

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;

            if (projectileSpawner != null)
            {
                projectileSpawner.UpdateSpawnTable(currentPhase);
            }
        }
    }

    public void AddIcon()
    {
        iconsCollected++;
        if (iconsCollected > maxIconsForGrade)
        {
            iconsCollected = maxIconsForGrade;
        }

        UpdateUI();
    }

    public void LoseLevel()
    {
        if (levelEnded) return;

        levelEnded = true;

        if (resultText != null)
        {
            resultText.text = "Burnout reached 0\nReturning to Hub...";
        }

        Invoke(nameof(ReturnToHub), 2f);
    }

    public void WinLevel()
    {
        if (levelEnded) return;

        levelEnded = true;

        string grade = CalculateGrade();

        if (resultText != null)
        {
            resultText.text = "You Survived!\nGrade: " + grade;
        }

        Invoke(nameof(ReturnToHub), 3f);
    }

    private string CalculateGrade()
    {
        float percent = (float)iconsCollected / maxIconsForGrade;

        if (percent >= 0.90f) return "A";
        if (percent >= 0.75f) return "B";
        if (percent >= 0.60f) return "C";
        if (percent >= 0.45f) return "D";
        return "F";
    }

    private void ReturnToHub()
    {
        SceneManager.LoadScene(hubSceneName);
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        if (healthText != null && player != null)
        {
            healthText.text = "Health: " + player.CurrentHealth;
        }

        if (iconText != null)
        {
            iconText.text = "Icons: " + iconsCollected + "/" + maxIconsForGrade;
        }

        if (phaseText != null)
        {
            phaseText.text = "Phase: " + currentPhase;
        }
    }
}