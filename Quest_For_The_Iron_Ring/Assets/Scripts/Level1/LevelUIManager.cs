using UnityEngine;
using TMPro;

public class LevelUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI piecesText;

    [Header("Level Settings")]
    public int totalPieces = 9;

    private int collectedPieces = 0;
    private float timer = 0f;
    private bool timerRunning = true;
    private bool allPiecesCollected = false;

    private void Start()
    {
        UpdatePiecesUI();
        UpdateTimerUI();
    }

    private void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        if (timerText != null)
        {
            timerText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    private void UpdatePiecesUI()
    {
        if (piecesText != null)
        {
            piecesText.text = "Pieces Found: " + collectedPieces + " / " + totalPieces;
        }
    }

    public void CollectPiece()
    {
        collectedPieces++;
        UpdatePiecesUI();

        if (collectedPieces >= totalPieces)
        {
            allPiecesCollected = true;
        }
    }

    public bool AreAllPiecesCollected()
    {
        return allPiecesCollected;
    }

    public int GetCollectedPieces()
    {
        return collectedPieces;
    }

    public float GetCurrentTime()
    {
        return timer;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
}