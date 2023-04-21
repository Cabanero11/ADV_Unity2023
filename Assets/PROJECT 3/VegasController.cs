using UnityEngine;

public class VegasController : MonoBehaviour
{
    // Public variables for adjusting in the inspector
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;


    
    private Rigidbody rb;
    private Animator animator;
    private float moveSpeed;
    public bool isGrounded;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Set movement speed based on input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        // Set animator parameters for movement and state
        animator.SetFloat("Speed", Mathf.Abs(vertical));
        animator.SetFloat("Direction", horizontal);

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Move the player based on input
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        movement.Normalize();
        movement *= moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + transform.TransformDirection(movement));
    }

    void Jump()
    {
        if (isGrounded)
        {
            // Cast a ray downwards to check if there's ground beneath the player
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundLayer))
            {
                // Jump and set animator parameters
                animator.SetTrigger("Jump");
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

}
