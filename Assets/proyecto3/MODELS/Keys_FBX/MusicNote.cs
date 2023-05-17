using UnityEngine;

public class MusicNote : MonoBehaviour
{
    public int scoreValue = 1;
    public AudioClip collectedSound;
    public ParticleSystem collectedParticles;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;

            // Play the collected sound
            AudioSource.PlayClipAtPoint(collectedSound, transform.position);

            // Increase the player's score
            ScoreManager.Instance.AddScore(scoreValue);

            // Emit the collected particles
            if (collectedParticles != null)
            {
                Instantiate(collectedParticles, transform.position, Quaternion.identity);
            }

            // Destroy the music note
            Destroy(gameObject);
        }
    }
}