using UnityEngine;

public class CursorMovement : MonoBehaviour
{
    public float speed = 200f;
    public float sensitivity = 0.3f;

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 move = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            move.y += 1;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            move.y -= 1;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            move.x -= 1;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            move.x += 1;

        if (move.magnitude > 1)
            move.Normalize();

        move *= sensitivity;

        rect.anchoredPosition += move * speed * Time.deltaTime;

        // Clamp to screen
        RectTransform canvasRect = rect.parent.GetComponent<RectTransform>();

        float halfCursorWidth = rect.rect.width * rect.localScale.x / 2f;
        float halfCursorHeight = rect.rect.height * rect.localScale.y / 2f;

        float minX = -canvasRect.rect.width / 2f + halfCursorWidth;
        float maxX = canvasRect.rect.width / 2f - halfCursorWidth;
        float minY = -canvasRect.rect.height / 2f + halfCursorHeight;
        float maxY = canvasRect.rect.height / 2f - halfCursorHeight;

        Vector2 clampedPosition = rect.anchoredPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        rect.anchoredPosition = clampedPosition;
    }
}