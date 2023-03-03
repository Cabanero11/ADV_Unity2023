using UnityEngine;

public class MoverCapsula : MonoBehaviour
{
    
    [Header("Cosas a lanzar")]
    public GameObject capsule;
    public float moveForce = 10f; // Force applied to the ball

    private Rigidbody rb;
    private Camera mainCamera;

    void Start()
    {
        rb = capsule.GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 launchDirection = mainCamera.transform.position - transform.position;
            rb.AddForce(launchDirection.normalized * moveForce, ForceMode.Impulse);
        }
    }
}