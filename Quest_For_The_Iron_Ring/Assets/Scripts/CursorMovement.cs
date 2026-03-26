using UnityEngine;

public class CursorMovement : MonoBehaviour
{
    public float speed = 150f;
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

        rect.anchoredPosition += move * speed * Time.deltaTime;
    }
}