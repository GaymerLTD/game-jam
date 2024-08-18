using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string playerTag = "Player"; // Tag used to find the player
    public Vector3 offset;    // Offset from the player
    public float smoothSpeed = 0.125f;  // Speed at which the camera follows the player

    private Transform player; // Cached reference to the player's transform

    private void LateUpdate()
    {
        player = GameObject.FindWithTag(playerTag).transform;
        // Calculate the desired position
        Vector3 desiredPosition = player.position + offset;
        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
