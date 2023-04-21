using UnityEngine;

public class Controller : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator component
    public float moveSpeed = 6f;  // Movement speed of the character
    public float rotationSpeed = 10f;  // Rotation speed of the character

    private Rigidbody rb;  // Reference to the Rigidbody component
    private float h;  // Horizontal input
    private float v;  // Vertical input

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get horizontal and vertical input
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // Set animator parameters for movement and state
        animator.SetFloat("H", h);
        animator.SetFloat("V", v);

        // Rotate the character towards the input direction
        if (h != 0f || v != 0f)
        {
            Vector3 direction = new Vector3(h, 0f, v);
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // Move the character based on input
        Vector3 movement = new Vector3(h, 0f, v).normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movement);
    }
}
