using UnityEngine;
using System.Collections.Generic;

public class UIButtonGroup : MonoBehaviour
{
    private List<UIButtonSelector> buttons = new List<UIButtonSelector>();
    private UIButtonSelector selectedButton;

    public void RegisterButton(UIButtonSelector button)
    {
        if (!buttons.Contains(button))
        {
            buttons.Add(button);
        }
    }

    public void SelectButton(UIButtonSelector button)
    {
        if (selectedButton != null && selectedButton != button)
        {
            selectedButton.Deselect();
        }

        selectedButton = button;
    }
}