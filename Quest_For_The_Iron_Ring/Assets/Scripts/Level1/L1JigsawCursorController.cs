using UnityEngine;
using UnityEngine.InputSystem;

public class L1JigsawCursorController : MonoBehaviour
{
    [SerializeField] private float cursorSpeed = 5f;
    [SerializeField] private float screenPadding = 0.5f;

    private Vector2 moveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.position += movement * Time.deltaTime * cursorSpeed;

        ClampToCameraView();
    }

    private void ClampToCameraView()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -camWidth + screenPadding, camWidth - screenPadding);
        pos.y = Mathf.Clamp(pos.y, -camHeight + screenPadding, camHeight - screenPadding);
        pos.z = -2f;

        transform.position = pos;
    }
}