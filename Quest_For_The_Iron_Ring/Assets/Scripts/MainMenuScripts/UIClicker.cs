using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIClicker : MonoBehaviour
{
    public RectTransform cursor;
    public CursorAnimator cursorAnimator;

    private PointerEventData pointer;
    private GameObject currentHovered;

    void Start()
    {
        pointer = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        pointer.position = cursor.position;

        List<RaycastResult> hoverResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, hoverResults);

        GameObject newHovered = hoverResults.Count > 0 ? hoverResults[0].gameObject : null;

        if (newHovered != currentHovered)
        {
            if (currentHovered != null)
            {
                ExecuteEvents.Execute(currentHovered, pointer, ExecuteEvents.pointerExitHandler);
            }

            if (newHovered != null)
            {
                ExecuteEvents.Execute(newHovered, pointer, ExecuteEvents.pointerEnterHandler);
            }

            currentHovered = newHovered;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (cursorAnimator != null)
            {
                cursorAnimator.PlayClickAnimation();
            }

            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = cursor.position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);

            if (results.Count > 0)
            {
                ExecuteEvents.Execute(results[0].gameObject, pointer, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}