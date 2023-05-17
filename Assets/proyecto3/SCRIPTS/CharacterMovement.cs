using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Variables Movimiento")]
    public float speed = 7f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public float jumpHeight = 1.2f;
    public float jumpSpeed = 6f;
    public float gravity = 18.18f;
    public float doubleJumpHeight = 1.4f;
    public float tripleJumpHeight = 1.6f;
    public int maxJumps = 2;
    public float jumpForceMultiplier = 1.3f;

    public float maxSpeed = 12f;
    public float minSpeed = 6f;
    public float speedSmoothTime = 0.2f;

    [Header("Death Zone")]
    public float respawnDelay = 2f;
    public float killHeight = -30f;
    public GameObject deathEffect;
    public Transform respawnPoint;

    private bool isRespawning = false;
    //private bool isFalling = false;

    [Header("Audio")]
    public AudioClip firstJumpAudioClip;
    public AudioClip doubleJumpAudioClip;
    public AudioClip tripleJumpAudioClip;
    public CinemachineFreeLook cinemachineCamera;


    [Header("Super Salto")]
    public float superJumpSpeed = 2f;
    public AudioClip superJumpAudioClip;

    private Animator animator;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpsLeft;



    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        characterController = gameObject.GetComponent<CharacterController>();
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        // Get input for movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Calculate the direction to move based on camera angle and input
        Vector3 direction = CalculateMoveDirection(h, v);

        // Check if the character is on the ground
        isGrounded = characterController.isGrounded;

        // Move the character
        Move(direction);

        // Handle jumping
        HandleJumping();

        if (transform.position.y < killHeight)
        {
            // Only respawn if not already respawning
            if (!isRespawning)
            {
                RespawnAfterDelay();
            }
        }
    }

    private void Move(Vector3 direction)
    {
        
        if (direction.magnitude >= 0.1f)
        {
            // Calculate target angle for rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cinemachineCamera.transform.eulerAngles.y;

            // Smoothly rotate the character
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Gradually increase the speed over time
            speed = Mathf.Lerp(speed, maxSpeed, speedSmoothTime * Time.deltaTime);

            // Move the character in the direction they're facing
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
        else 
        {
            // Gradually decrease the speed over time if not moving
            speed = Mathf.Lerp(speed, minSpeed, speedSmoothTime * Time.deltaTime);
        }

        animator.SetFloat("speed", characterController.velocity.magnitude);
        //Debug.Log("speed:" + characterController.velocity.magnitude);

    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || jumpsLeft > 0))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * gravity);
                jumpsLeft = maxJumps - 1;

                // Play first jump audio clip
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.clip = firstJumpAudioClip;
                audioSource.Play();
                animator.SetBool("Jump", true);
                animator.SetBool("Jump2", false);
                animator.SetBool("Jump3", false);
            }
            else if (jumpsLeft == 2)
            {
                velocity.y = Mathf.Sqrt(2f * doubleJumpHeight * gravity);
                jumpsLeft--;

                // Play second jump audio clip
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.clip = doubleJumpAudioClip;
                audioSource.Play();
                animator.SetBool("Jump", true);
                animator.SetBool("Jump2", true);
                animator.SetBool("Jump3", false);
            }
            else if (jumpsLeft == 1)
            {
                velocity.y = Mathf.Sqrt(2f * tripleJumpHeight * gravity);
                jumpsLeft--;

                // Play triple jump audio clip
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.clip = tripleJumpAudioClip;
                audioSource.Play();
                animator.SetBool("Jump", true);
                animator.SetBool("Jump2", true);
                animator.SetBool("Jump3", true);
            }
        } 
        else if(isGrounded)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Jump2", false);
            animator.SetBool("Jump3", false);
        }

        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;

        // Move character with gravity
        characterController.Move(velocity * Time.deltaTime);
    }

    private Vector3 CalculateMoveDirection(float h, float v)
    {
        // Calculate the direction to move based on camera angle and input
        Vector3 direction = Vector3.zero;

        if (cinemachineCamera != null)
        {
            // Get the forward direction of the camera
            Vector3 cameraForward = cinemachineCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            // Get the right direction of the camera
            Vector3 cameraRight = cinemachineCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // Calculate the move direction based on camera forward and right directions
            direction = cameraForward * v + cameraRight * h;
        }
        else
        {
            // Calculate the move direction based on world up/down/left/right directions
            direction = Vector3.forward * v + Vector3.right * h;
        }

        // Set the move direction to be in the same direction as forward
        direction = Quaternion.Euler(0, transform.eulerAngles.y, 0) * direction;

        return direction;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            //isFalling = true;
            animator.SetBool("Falling", true);
            StartCoroutine(RespawnAfterDelay());
        }
    }

    public IEnumerator RespawnAfterDelay()
    {
        //isFalling = false;
        animator.SetBool("Falling", false);

        // Set respawning flag
        isRespawning = true;

        // Play death effect
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Disable player movement and hide the character
        this.GetComponentInChildren<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Reset the player position and re-enable movement and character visibility
        transform.position = respawnPoint.position;
        this.GetComponentInChildren<MeshRenderer>().enabled = true;


        isRespawning = false;

    }


}

