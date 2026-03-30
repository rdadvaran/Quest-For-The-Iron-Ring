using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleCursorController : MonoBehaviour
{
    [SerializeField] private float cursorSpeed = 5f;
    private Vector2 _moveInput;
    
    public Vector2 MoveInput => _moveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(_moveInput.x, _moveInput.y, 0f);
        transform.position += (movement * Time.deltaTime * cursorSpeed);
    }
}
