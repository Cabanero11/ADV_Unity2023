using UnityEngine;

public class AnimacionCaņon : MonoBehaviour
{
    public GameObject theCannon;
    public AudioClip audioClip;
    private AudioSource audioSource;
    private Animator animator;

    [Header("Variable de cooldown entre disparos")]
    [Range(0f,1f)]
    public float cooldownTime; // the length of the cooldown in seconds
    private float lastClickTime; // the time of the last click

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - lastClickTime > cooldownTime)
            {
                // record the time of the click
                lastClickTime = Time.time;

                animator.SetBool("CaņonDispara", true);
                PlayAnimationAndSound();
            } 
        }
        else
        {
            animator.SetBool("CaņonDispara", false);
        }
    }

    private void PlayAnimationAndSound()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
