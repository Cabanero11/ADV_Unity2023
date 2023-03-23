using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public GameObject particlePrefab; // Assign the particle system prefab in the Inspector
    public SphereCollider sphereCollider;
    public float timetoDestroy = 1.4f;
    [Header("Sonido de bola chochando con caja")]
    public AudioClip audioClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Crates"))
        {
            PlayBallSound();
            sphereCollider.isTrigger = false;
            ScoreSystem.Instance.AddScore(5);
            //Debug.Log(ScoreSystem.Instance.scoreText.text);
            Instantiate(particlePrefab, collision.ClosestPoint(transform.position), Quaternion.identity);
            //Destroy(particlePrefab, timetoDestroy);
        }
    }

    private void PlayBallSound()
    {
        audioSource.Play();
    }
}
