using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIClicker : MonoBehaviour
{
    public RectTransform cursor;
    public CursorAnimator cursorAnimator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (cursorAnimator != null){
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