using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [Header("Respawn Variables")]
    public Transform respawnPoint; // The location the player will respawn at
    public float killHeight = -16f; // The height at which the player will die and respawn
    public float respawnDelay = 2f; // The time delay before the player respawns
    public GameObject deathEffect; // The particle effect to play when the player dies

    private bool isRespawning = false;

    private void Update()
    {
        // Check if the player has fallen below the kill height
        if (transform.position.y < killHeight)
        {
            // Only respawn if not already respawning
            if (!isRespawning)
            {
                StartCoroutine(Respawn());
            }
        }
    }

    IEnumerator Respawn()
    {
        // Set respawning flag
        isRespawning = true;

        // Play death effect
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Disable player movement and hide the character
        GetComponent<MeshRenderer>().enabled = false;

        // Wait for respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Reset the player position and re-enable movement and character visibility
        transform.position = respawnPoint.position;
        GetComponent<MeshRenderer>().enabled = true;

        // Set respawning flag back to false
        isRespawning = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a red wireframe cube to show the killzone
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
