using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float fixedY = 0f;
    [SerializeField] private float fixedZ = -10f;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    private void LateUpdate()
    {
        if (target == null) return;

        float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
        Vector3 desiredPosition = new Vector3(clampedX, fixedY, fixedZ);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}