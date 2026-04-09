using UnityEngine;
using UnityEngine.UI;

public class Burnout_UI : MonoBehaviour
{
    [SerializeField] private Image burnoutImage;
    [SerializeField] private Sprite[] burnoutSprites;

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        int level = GameManager.Instance.GetBurnoutLevel();

        if (burnoutImage != null && burnoutSprites != null && level >= 0 && level < burnoutSprites.Length)
        {
            burnoutImage.sprite = burnoutSprites[level];
        }
    }
}