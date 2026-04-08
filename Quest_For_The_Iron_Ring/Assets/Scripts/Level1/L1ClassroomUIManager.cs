using UnityEngine;
using TMPro;

public class L1ClassroomUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI piecesText;

    [Header("References")]
    public L1ClassroomPieceSpawner pieceSpawner;

    private int totalPieces = 0;
    private bool allPiecesCollected = false;

    private void Start()
    {
        if (pieceSpawner != null)
        {
            totalPieces = pieceSpawner.GetPieceCount();
        }
        else
        {
            totalPieces = 6;
            Debug.LogWarning("PieceSpawner reference is missing in L1ClassroomUIManager.");
        }

        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.totalPieces = totalPieces;
            GlobalGameManager.Instance.collectedPieces = 0;
        }

        UpdatePiecesUI();
        UpdateTimerUI();
    }

    private void Update()
    {
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null && GlobalGameManager.Instance != null)
        {
            int minutes = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime / 60f);
            int seconds = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime % 60f);

            timerText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    private void UpdatePiecesUI()
    {
        if (piecesText != null && GlobalGameManager.Instance != null)
        {
            piecesText.text = "Pieces Found: " + GlobalGameManager.Instance.collectedPieces + " / " + totalPieces;
        }
    }

    public void CollectPiece()
    {
        if (GlobalGameManager.Instance == null) return;

        GlobalGameManager.Instance.collectedPieces++;
        UpdatePiecesUI();

        if (GlobalGameManager.Instance.collectedPieces >= totalPieces)
        {
            allPiecesCollected = true;
        }
    }

    public bool AreAllPiecesCollected()
    {
        return allPiecesCollected;
    }

    public int GetTotalPieces()
    {
        return totalPieces;
    }
}