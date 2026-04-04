using UnityEngine;
using TMPro;

public class LevelUIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI piecesText;

    public int totalPieces = 6;

    private int collectedPieces = 0;
    private float timer = 0f;
    private bool timerRunning = true;

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

        timerText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void Start()
    {
        UpdatePiecesUI();
    }

    private void UpdatePiecesUI()
    {
        piecesText.text = "Pieces Found: " + collectedPieces + " / " + totalPieces;
    }

    public void CollectPiece()
    {
        collectedPieces++;
        UpdatePiecesUI();
    }

    public int GetCollectedPieces()
    {
        return collectedPieces;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetTime()
    {
        return timer;
    }
}
