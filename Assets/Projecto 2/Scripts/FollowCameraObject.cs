using UnityEngine;

public class FollowCameraObject : MonoBehaviour
{
    [Header("Objeto a Seguir")]
    public Transform target; // The target object to follow
    public float smoothSpeed = 0.125f; // The smoothness of the camera movement
    public Vector3 offset; // The offset between the camera and the target

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset; // The desired position of the camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // The smoothed position of the camera
        transform.position = smoothedPosition; // Set the position of the camera to the smoothed position
    }
}
