using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VegasMovement : MonoBehaviour
{
    [Header("Variables")]
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    [Header("Salto")]
    public float jumpHeight = 1f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;
    public float doubleJumpHeight = 2f;
    public int maxJumps = 2;
    public float jumpForceMultiplier = 1.5f;
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
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0f, v).normalized;

        // Check if the character is on the ground
        isGrounded = characterController.isGrounded;
        float targetAngle, angle;
        Vector3 moveDirection;

        if (direction.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cinemachineCamera.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
            //animator.SetFloat("H", h);
            //animator.SetFloat("V", v);
        }

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
