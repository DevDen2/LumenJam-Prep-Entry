using UnityEngine;

public class CameraYAxisOnly : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public float zPosition = -10f;
    public float xPosition = 0f;

    [Header("Clamping Settings")]
    public float minY = -5f; // Lowest the camera can go
    public float maxY = 20f; // Highest the camera can go

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            // 1. Calculate the ideal Y position based on the target
            float targetY = target.position.y;

            // 2. Clamp that Y position so it stays within your bounds
            float clampedY = Mathf.Clamp(targetY, minY, maxY);

            // 3. Create the final destination vector
            Vector3 targetPosition = new Vector3(xPosition, clampedY, zPosition);

            // 4. Smoothly move to that clamped destination
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
