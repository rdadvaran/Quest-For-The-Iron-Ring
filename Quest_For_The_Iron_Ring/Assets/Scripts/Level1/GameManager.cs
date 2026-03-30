using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform cursor;
    
    [Header("Game Elements")] 
    [Range(2, 9)] 
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform piecePrefab;
    
    [Header("UI Elements")]
    [SerializeField] private Texture2D puzzleTexture;

    void Start()
    {
        StartGame(puzzleTexture);
    }
    
    private  List<Transform> pieces;
    private Vector2Int dimensions;
    private float width;
    private float height;

    private Transform draggingPiece = null;
    //   private Vector3 offset;
    
    //Update is called once per frame
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
        //we store a list of the transform for each jigsaw piece so we can track them later
        pieces = new List<Transform>();
        
        // Calculate the size of each jigsaw piece, based on difficulty setting
        dimensions = GetDimensions(jigsawTexture, difficulty);
        
        //Create the pieces of the correct size with the correct texture
        CreateJigsawPieces(jigsawTexture);
        
        //Place the pieces randomly into the visible area.
        Scatter();
        
        //Update the border to fit the puzzle
        UpdateBorder();
    }

    Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty)
    {
        Vector2Int dimensions = Vector2Int.zero;
        
        //Diffilculty is the number of pieces on the smallest texture dimension
        //This helps ensure the pieces are as sqaures as possible
        if (jigsawTexture.width < jigsawTexture.height)
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
        }
        else
        {
            dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
            dimensions.y = difficulty;
        }

            return dimensions;
    }

    void CreateJigsawPieces(Texture2D jigsawTexture)
    {
        // Calculate piece sizes based on the dimensions
        height = 1f / dimensions.y;
        float aspect = (float)jigsawTexture.width / jigsawTexture.height;
        width = aspect / dimensions.x;

        for (int row = 0; row < dimensions.y; row++) {
            for (int col = 0; col < dimensions.x; col++)
            {
                // Create the puece ijn the right location of the right size
                Transform piece = Instantiate(piecePrefab, gameHolder);
                piece.localPosition = new Vector3(
                    (-width * dimensions.x / 2) + (width * col) + (width / 2),
                    (-height * dimensions.y / 2) + (height * row) + (height / 2),
                    -1);
                piece.localScale = new Vector3(width, height, 1f);
                
                // Don't have to name it but better for debugging
                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);
                
                // assign the correct part of the etxture for this jigsaw piece
                // We need our width and height both to be normalised between 0 and 1 for the UV.

                float width1 = 1f / dimensions.x;
                float height1 = 1f / dimensions.y;
                
                //UV coord order is anti-clockwise: (0,0),(0,1),(1,0), (1,1)
                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1 * col, height1 * row);
                uv[1] = new Vector2(width1 * (col + 1), height1 * row);
                uv[2] = new Vector2(width1 * col , height1 * (row + 1));
                uv[3] = new Vector2(width1 * (col + 1), height1 * (row + 1));
                
                // Assign our new UVs to the mesh.
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;
                
                //Update the texture on the piece
                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);

            }
        }
    }

    private void Scatter()
    {
        //Calculate the visible orthographic size of the screen.
        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (orthoHeight * screenAspect);
        
        //Ensure pieces are away from the edges
        float pieceWidth = width * gameHolder.localScale.x;
        float pieceHeight = height * gameHolder.localScale.y;
        
        orthoHeight -= pieceHeight;
        orthoWidth -= pieceWidth;
        

        // Place each piece randomly in the visible area
        foreach (Transform piece in pieces) {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            piece.position = new Vector3(x,y,-1);
        }
    }

    private void UpdateBorder()
    {
        LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();
        
        // Calculate half sizes to simplify the code
        float halfWidth = (width * dimensions.x) / 2f;
        float halfHeight = (height * dimensions.y) / 2f;

        float borderZ = 0f;
        
        // Set border vertices, starting top left, going clockwise
        lineRenderer.SetPosition(0,new Vector3(-halfWidth,halfHeight,borderZ));
        lineRenderer.SetPosition(1,new Vector3(halfWidth,halfHeight,borderZ));
        lineRenderer.SetPosition(2,new Vector3(halfWidth,-halfHeight,borderZ));
        lineRenderer.SetPosition(3,new Vector3(-halfWidth,-halfHeight,borderZ));
        
        //Set the thickness of the border line.
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        
        //Show the border line.
        lineRenderer.enabled = true;
    }

    private void SnapAndDisableIfCorrect() {
        // We need to know the index of the piece to determine its correct position
        int pieceIndex = pieces.IndexOf(draggingPiece);
        
        // The coordinates of the pieces in the puzzle.
        int col = pieceIndex % dimensions.x;
        int row = pieceIndex / dimensions.x;
        
        // The target position in the non-scaled coordinates
        Vector2 targetPosition = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
            (-height * dimensions.y / 2) + (height * row) + (height / 2));
        
        //Check if were in the correct location.
        if (Vector2.Distance(draggingPiece.localPosition, targetPosition) < (width / 2))
        {
            draggingPiece.localPosition = targetPosition;
            
            //Disable the collider so we can't click on the object anymore.
            draggingPiece.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    
    
}
