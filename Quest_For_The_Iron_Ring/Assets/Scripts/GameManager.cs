using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Elements")] 
    [Range(2, 9)] 
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform piecePrefab;
    
    [Header("UI Elements")]
    [SerializeField] private List<Texture2D> imagesTextures;
    [SerializeField] private Transform levelSelectionPanel;
    [SerializeField] private Image levelSelectPrefab;

    void Start()
    {
        foreach (Texture2D texture in imagesTextures) {
            Image image = Instantiate(levelSelectPrefab, levelSelectionPanel);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),Vector2.zero);
            
            // assign button action
            image.GetComponent<Button>().onClick.AddListener(() => StartGame(texture));
        }
    }
    
    private  List<Transform> pieces;
    private Vector2Int dimensions;
    private float width;
    private float height;


    public void StartGame(Texture2D jigsawTexture)
    {
        levelSelectionPanel.gameObject.SetActive(false);
        //we store a list of the transform for each jigsaw piece so we can track them later
        pieces = new List<Transform>();
        
        // Calculate the size of each jigsaw piece, based on difficulty setting
        dimensions = GetDimensions(jigsawTexture, difficulty);
        
        //Create the pieces of the correct size with the correct texture
        CreateJigsawPieces(jigsawTexture);
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
    
    
}
