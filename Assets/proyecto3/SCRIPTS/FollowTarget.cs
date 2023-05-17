using UnityEngine;
using UnityEngine.Playables;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 2.0f;


    private void Update()
    {

        // Calculate the direction to the target
        Vector3 direction = target.position - transform.position;

        // Move towards the target
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        // Rotate towards the target
        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(rotationOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

