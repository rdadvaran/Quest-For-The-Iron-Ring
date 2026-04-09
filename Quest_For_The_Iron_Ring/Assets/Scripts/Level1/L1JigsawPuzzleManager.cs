using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class L1JigsawPuzzleManager : MonoBehaviour
{
    [SerializeField] private Transform cursor;

    [Header("Game Elements")]
    [Range(2, 9)]
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform piecePrefab;

    [Header("Puzzle Data")]
    [SerializeField] private Texture2D puzzleTexture;

    [Header("UI")]
    [SerializeField] private L1JigsawUIManager puzzleUIManager;

    private List<Transform> pieces;
    private Vector2Int dimensions;
    private float width;
    private float height;

    private Transform draggingPiece = null;

    private void Start()
    {
        SetDifficultyFromGameSession();

        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.correctPlacements = 0;
        }

        StartGame(puzzleTexture);

        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.totalPieces = pieces.Count;
        }

        if (puzzleUIManager != null)
        {
            puzzleUIManager.UpdateCorrectUI();
        }
    }

    private void SetDifficultyFromGameSession()
    {
        difficulty = 3; // default

        if (GameSession.Instance != null)
        {
            string selectedDifficulty = GameSession.Instance.selectedDifficulty;
            Debug.Log("Jigsaw selected difficulty: " + selectedDifficulty);

            switch (selectedDifficulty)
            {
                case "Idle Slacker":
                    difficulty = 2;
                    break;

                case "Average Joe":
                    difficulty = 3;
                    break;

                case "Goodie 2 Shoes":
                    difficulty = 3;
                    break;

                case "Perfectionist":
                    difficulty = 4;
                    break;

                default:
                    difficulty = 3;
                    break;
            }
        }

        Debug.Log("Jigsaw puzzle difficulty value = " + difficulty);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryPickUpPiece();
        }

        if (draggingPiece != null && Keyboard.current.spaceKey.isPressed)
        {
            draggingPiece.position = new Vector3(cursor.position.x, cursor.position.y, -1f);
        }

        if (draggingPiece != null && Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            SnapAndDisableIfCorrect();
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }
    }

    private void TryPickUpPiece()
    {
        Collider2D hit = Physics2D.OverlapPoint(cursor.position);

        if (hit != null && pieces.Contains(hit.transform))
        {
            draggingPiece = hit.transform;
        }
    }

    public void StartGame(Texture2D jigsawTexture)
    {
        pieces = new List<Transform>();

        dimensions = GetDimensions(jigsawTexture, difficulty);

        CreateJigsawPieces(jigsawTexture);
        Scatter();
        UpdateBorder();
    }

    private Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty)
    {
        Vector2Int dims = Vector2Int.zero;

        if (jigsawTexture.width < jigsawTexture.height)
        {
            dims.x = difficulty;
            dims.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
        }
        else
        {
            dims.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
            dims.y = difficulty;
        }

        return dims;
    }

    private void CreateJigsawPieces(Texture2D jigsawTexture)
    {
        height = 1f / dimensions.y;
        float aspect = (float)jigsawTexture.width / jigsawTexture.height;
        width = aspect / dimensions.x;

        for (int row = 0; row < dimensions.y; row++)
        {
            for (int col = 0; col < dimensions.x; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameHolder);

                piece.localPosition = new Vector3(
                    (-width * dimensions.x / 2) + (width * col) + (width / 2),
                    (-height * dimensions.y / 2) + (height * row) + (height / 2),
                    -1f
                );

                piece.localScale = new Vector3(width, height, 1f);
                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);

                float width1 = 1f / dimensions.x;
                float height1 = 1f / dimensions.y;

                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1 * col, height1 * row);
                uv[1] = new Vector2(width1 * (col + 1), height1 * row);
                uv[2] = new Vector2(width1 * col, height1 * (row + 1));
                uv[3] = new Vector2(width1 * (col + 1), height1 * (row + 1));

                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;

                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);
            }
        }
    }

    private void Scatter()
    {
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = orthoHeight * screenAspect;

        float pieceWidth = width * gameHolder.localScale.x;
        float pieceHeight = height * gameHolder.localScale.y;

        orthoHeight -= pieceHeight;
        orthoWidth -= pieceWidth;

        foreach (Transform piece in pieces)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            piece.position = new Vector3(x, y, -1f);
        }
    }

    private void UpdateBorder()
    {
        LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();

        float halfWidth = (width * dimensions.x) / 2f;
        float halfHeight = (height * dimensions.y) / 2f;
        float borderZ = 0f;

        lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, borderZ));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, borderZ));

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = true;
    }

    private void SnapAndDisableIfCorrect()
    {
        if (draggingPiece == null) return;

        int pieceIndex = pieces.IndexOf(draggingPiece);

        int col = pieceIndex % dimensions.x;
        int row = pieceIndex / dimensions.x;

        Vector2 targetPosition = new Vector2(
            (-width * dimensions.x / 2) + (width * col) + (width / 2),
            (-height * dimensions.y / 2) + (height * row) + (height / 2)
        );

        if (Vector2.Distance(draggingPiece.localPosition, targetPosition) < (width / 2))
        {
            BoxCollider2D pieceCollider = draggingPiece.GetComponent<BoxCollider2D>();

            if (pieceCollider != null && pieceCollider.enabled)
            {
                draggingPiece.localPosition = targetPosition;
                pieceCollider.enabled = false;

                if (puzzleUIManager != null)
                {
                    puzzleUIManager.AddCorrectPlacement();
                }

                if (GlobalGameManager.Instance != null &&
                    GlobalGameManager.Instance.correctPlacements >= pieces.Count)
                {
                    if (puzzleUIManager != null)
                    {
                        puzzleUIManager.ShowFinishPanel();
                    }
                }
            }
        }
    }
}