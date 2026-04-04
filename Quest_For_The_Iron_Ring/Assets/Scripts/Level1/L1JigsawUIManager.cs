using UnityEngine;
using TMPro;

public class L1JigsawUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI correctText;

    [Header("Puzzle Settings")]
    public int totalPieces = 9;

    private void Start()
    {
        UpdateTimerUI();
        UpdateCorrectUI();
    }

    private void Update()
    {
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null || GlobalGameManager.Instance == null)
            return;

        int minutes = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime / 60f);
        int seconds = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime % 60f);

        timerText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void AddCorrectPlacement()
    {
        if (GlobalGameManager.Instance == null) return;

        GlobalGameManager.Instance.correctPlacements++;
        UpdateCorrectUI();
    }

    private void UpdateCorrectUI()
    {
        if (correctText != null && GlobalGameManager.Instance != null)
        {
            correctText.text = "Correct Placements: " + GlobalGameManager.Instance.correctPlacements + " / " + totalPieces;
        }
    }
}