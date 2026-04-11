using UnityEngine;
using UnityEngine.UI;

public class ShapeJamBurnoutUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image burnoutImage;
    [SerializeField] private PlayerController3 player;

    [Header("Burnout Frames")]
    [SerializeField] private Sprite[] burnoutSprites;

    private void Start()
    {
        if (burnoutImage == null)
        {
            burnoutImage = GetComponent<Image>();
        }

        if (player == null)
        {
            player = FindObjectOfType<PlayerController3>();
        }

        UpdateBurnoutSprite();
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController3>();
        }

        UpdateBurnoutSprite();
    }

    private void UpdateBurnoutSprite()
    {
        if (burnoutImage == null || player == null || burnoutSprites == null || burnoutSprites.Length == 0)
        {
            return;
        }

        int missingHealth = player.MaxHealth - player.CurrentHealth;
        missingHealth = Mathf.Clamp(missingHealth, 0, burnoutSprites.Length - 1);

        burnoutImage.sprite = burnoutSprites[missingHealth];
    }
}