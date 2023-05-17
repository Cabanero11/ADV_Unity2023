using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CharacterMovement2 : MonoBehaviour
{
    [Header("Variables Movimiento y Salto")]
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

    [Header("Salto particulas")]
    public GameObject firstJumpParticles;
    public GameObject doubleJumpParticles;
    public GameObject tripleJumpParticles;


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
    public float superJumpHeight = 20f;
    public float superJumpDuration = 2f;
    public AudioSource superJumpAudioClip;
    private bool isSuperJumping = false;
    public GameObject superJumpParticles;
    public SphereCollider sphereCollider;

    private Animator animator;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpsLeft;

    [Header("Timeline")]
    public PlayableDirector timelineDirector1, timelineDirector2, timelineDirector3;
    private bool isTimelinePlaying = false;
    public GameObject Guitar2;



    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        characterController = gameObject.GetComponent<CharacterController>();
        sphereCollider = GetComponent<SphereCollider>();
        jumpsLeft = maxJumps;
        sphereCollider.enabled = false;
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

        if (isSuperJumping)
        {
            //isSuperJumping = false;
            animator.SetBool("SuperJump", true);
            characterController.Move(Vector3.up * superJumpHeight * Time.deltaTime);
        }

        // Check if timeline is playing
        if (timelineDirector1.state == PlayState.Playing)
        {
            // If timeline is playing and movement is enabled, disable movement
            if (!isTimelinePlaying && characterController.enabled)
            {
                characterController.enabled = false;
                isTimelinePlaying = true;
            }
        }
        else if (timelineDirector2.state == PlayState.Playing)
        {
            // If timeline is playing and movement is enabled, disable movement
            if (!isTimelinePlaying && characterController.enabled)
            {
                characterController.enabled = false;
                isTimelinePlaying = true;
                Guitar2.SetActive(true);
            }

        }
        else if (timelineDirector3.state == PlayState.Playing)
        {
            // If timeline is playing and movement is enabled, disable movement
            if (!isTimelinePlaying && characterController.enabled)
            {
                characterController.enabled = false;
                isTimelinePlaying = true;
            }
        }
        else
        {
            // If timeline is not playing and movement is disabled, enable movement
            if (isTimelinePlaying && !characterController.enabled)
            {
                characterController.enabled = true;
                isTimelinePlaying = false;
                Debug.Log("you can play now");
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

                if (firstJumpParticles != null)
                {
                    GameObject particlesGO = Instantiate(firstJumpParticles, transform.position, Quaternion.identity);
                    ParticleSystem particles = particlesGO.GetComponent<ParticleSystem>();
                    if (particles != null)
                    {
                        particles.Play();
                    }
                    else
                    {
                        Debug.LogWarning("No ParticleSystem component found on instantiated GameObject.");
                    }
                }
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

                if (doubleJumpParticles != null)
                {
                    GameObject particlesGO = Instantiate(doubleJumpParticles, transform.position, Quaternion.identity);
                    ParticleSystem particles = particlesGO.GetComponent<ParticleSystem>();
                    if (particles != null)
                    {
                        particles.Play();
                    }
                    else
                    {
                        Debug.LogWarning("No ParticleSystem component found on instantiated GameObject.");
                    }
                }
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

                if(tripleJumpParticles != null)
                {
                    GameObject particlesGO = Instantiate(tripleJumpParticles, transform.position, Quaternion.identity);
                    ParticleSystem particles = particlesGO.GetComponent<ParticleSystem>();
                    if (particles != null)
                    {
                        particles.Play();
                    }
                    else
                    {
                        Debug.LogWarning("No ParticleSystem component found on instantiated GameObject.");
                    }
                }
            }
        } 
        else if(isGrounded)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Jump2", false);
            animator.SetBool("Jump3", false);
            animator.SetBool("SuperJump", false);
            sphereCollider.enabled = false;
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
        if (deathEffect != null)
        {
            GameObject particlesGO = Instantiate(deathEffect, transform.position, Quaternion.identity);
            ParticleSystem particles = particlesGO.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
            }
            else
            {
                Debug.LogWarning("No ParticleSystem component found on instantiated GameObject.");
            }
        }

        // Disable player movement and hide the character
        this.GetComponentInChildren<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Reset the player position and re-enable movement and character visibility
        transform.position = respawnPoint.position;
        this.GetComponentInChildren<MeshRenderer>().enabled = true;


        isRespawning = false;

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "SuperSalto")
        {
            if (isGrounded)
            {
                isSuperJumping = true;
                //Debug.Log("isSuperJumping = " + isSuperJumping);
                float jumpVelocity = Mathf.Sqrt(2 * superJumpHeight * -Physics.gravity.y);
                characterController.Move(Vector3.up * jumpVelocity * superJumpDuration);
                sphereCollider.enabled = true;

                if (superJumpParticles != null)
                {
                    GameObject particlesGO = Instantiate(superJumpParticles, transform.position, Quaternion.identity);
                    ParticleSystem particles = particlesGO.GetComponent<ParticleSystem>();
                    if (particles != null)
                    {
                        particles.Play();
                    }
                    else
                    {
                        Debug.LogWarning("No ParticleSystem component found on instantiated GameObject.");
                    }
                }

                if (superJumpAudioClip != null)
                {
                    superJumpAudioClip.Play();
                }
            } 
        }

        if(hit.collider.tag == "DejarSalto")
        {
            isSuperJumping = false;
        }
    }






}

