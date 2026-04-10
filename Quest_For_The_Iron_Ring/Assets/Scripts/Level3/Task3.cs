using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Task3 : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private float levelTime = 180f;
    [SerializeField] private string hubSceneName = "Hallway";
    [SerializeField] private int maxIconsForGrade = 20;
    [SerializeField] public float FinalGrade = 0; // final grade for jami
    [SerializeField] private float endScreenDuration = 10f;

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

    [Header("End Screen UI")]
    [SerializeField] private GameObject endScreenRoot;
    [SerializeField] private GameObject winTitleImage;
    [SerializeField] private GameObject loseTitleImage;
    [SerializeField] private TMP_Text gradeText;

    private float currentTime;
    private bool levelEnded = false;
    private int currentPhase = 1;
    private int iconsCollected = 0;

    private PlayerController3 player;

    public int Phase => currentPhase;
    public int IconsCollected => iconsCollected;
    public int MaxIconsForGrade => maxIconsForGrade;

    private void Start()
    {
        currentTime = levelTime;

        FindPlayer();

        if (projectileSpawner != null)
        {
            projectileSpawner.UpdateSpawnTable(currentPhase);
        }

        HideEndScreen();
        UpdateUI();
    }

    private void Update()
    {
        if (levelEnded)
        {
            return;
        }

        if (player == null)
        {
            FindPlayer();
        }

        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
        {
            currentTime = 0f;
        }

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
            player = playerObject.GetComponent<PlayerController3>();
        }
    }

    private void UpdatePhase()
    {
        float elapsed = levelTime - currentTime;
        int newPhase = 1;

        if (elapsed >= phase3StartTime)
        {
            newPhase = 3;
        }
        else if (elapsed >= phase2StartTime)
        {
            newPhase = 2;
        }

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
        if (levelEnded)
        {
            return;
        }

        levelEnded = true;
        ShowLoseScreen();

        Invoke(nameof(ReturnToHub), endScreenDuration);
    }

    public void WinLevel()
    {
        if (levelEnded)
        {
            return;
        }

        levelEnded = true;
        ShowWinScreen();
        FinalGrade = ((float)iconsCollected / maxIconsForGrade) * 100f; // final grade of jami

        Invoke(nameof(ReturnToHub), endScreenDuration);
    }

    private void ShowLoseScreen()
    {
        if (endScreenRoot != null)
        {
            endScreenRoot.SetActive(true);
        }

        if (winTitleImage != null)
        {
            winTitleImage.SetActive(false);
        }

        if (loseTitleImage != null)
        {
            loseTitleImage.SetActive(true);
        }

        if (gradeText != null)
        {
            gradeText.gameObject.SetActive(false);
        }
    }

    private void ShowWinScreen()
    {
        if (endScreenRoot != null)
        {
            endScreenRoot.SetActive(true);
        }

        if (loseTitleImage != null)
        {
            loseTitleImage.SetActive(false);
        }

        if (winTitleImage != null)
        {
            winTitleImage.SetActive(true);
        }

        if (gradeText != null)
        {
            gradeText.gameObject.SetActive(true);

            float percent = 0f;

            if (maxIconsForGrade > 0)
            {
                percent = ((float)iconsCollected / maxIconsForGrade) * 100f;
            }

            gradeText.text = Mathf.RoundToInt(percent) + "%";
        }
    }

    private void HideEndScreen()
    {
        if (endScreenRoot != null)
        {
            endScreenRoot.SetActive(false);
        }

        if (winTitleImage != null)
        {
            winTitleImage.SetActive(false);
        }

        if (loseTitleImage != null)
        {
            loseTitleImage.SetActive(false);
        }

        if (gradeText != null)
        {
            gradeText.gameObject.SetActive(false);
        }
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