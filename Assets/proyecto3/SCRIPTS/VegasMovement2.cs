using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VegasMovement2 : MonoBehaviour
{
    [Header("Variables")]
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    [Header("Salto")]
    public float jumpHeight = 1.16f;
    public float jumpSpeed = 6.8f;
    public float gravity = 18.18f;
    public float doubleJumpHeight = 1.2f;
    public int maxJumps = 1;
    public float jumpForceMultiplier = 1.14f;
    public AudioClip jumpAudioClip;
    public CinemachineFreeLook cinemachineCamera;

    private float h, v;
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
        // Get the horizontal and vertical input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Get the direction the character is facing
        Vector3 direction = cinemachineCamera.transform.forward;
        direction.y = 0f;
        direction = direction.normalized;

        // Get the right vector of the character
        Vector3 right = new Vector3(direction.z, 0f, -direction.x);

        // Calculate the direction vector based on the input and character rotation
        Vector3 moveDirection = (h * right + v * direction).normalized;

        // Move the character in the direction of the moveDirection vector
        characterController.Move(moveDirection * speed * Time.deltaTime);

        // Check if space bar is pressed and character is on the ground or has jumps left
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || jumpsLeft > 0))
        {
            // Play jump audio clip
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.clip = jumpAudioClip;
            audioSource.Play();

            // Calculate the jump height based on whether this is a double jump or not
            float currentJumpHeight = isGrounded ? jumpHeight : doubleJumpHeight;

            // Set the vertical velocity based on the current jump height and the jump speed
            velocity.y = Mathf.Sqrt(currentJumpHeight * 2f * gravity) + jumpSpeed * jumpForceMultiplier;

            // Decrease the number of jumps left if this is not a regular jump
            if (!isGrounded)
            {
                jumpsLeft--;
            }
        }

        // Subtract gravity from the velocity to simulate falling
        velocity.y -= gravity * Time.deltaTime;

        // Add the velocity to the character's position
        characterController.Move(velocity * Time.deltaTime);

        // If the character is on the ground, reset the number of jumps left
        if (isGrounded)
        {
            jumpsLeft = maxJumps;
        }
    }
}
