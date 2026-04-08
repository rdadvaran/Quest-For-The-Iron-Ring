using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSelector : MonoBehaviour, IPointerClickHandler
{
    public UIButtonGroup group;
    public Color selectedColor = new Color(0.7f, 0.7f, 0.7f);

    private Image image;
    private Color originalColor;
    private bool isSelected = false;

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;

        if (group != null)
        {
            group.RegisterButton(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        if (group != null)
        {
            group.SelectButton(this);
        }

        isSelected = true;
        image.color = selectedColor;
    }

    public void Deselect()
    {
        isSelected = false;
        image.color = originalColor;
    }
}