using UnityEngine;

public class Ball : MonoBehaviour {
    public GameObject particlePrefab; // The prefab for the particle system

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") { // Check if the collision is with the ground
            ContactPoint contact = collision.contacts[0]; // Get the point of collision
            Instantiate(particlePrefab, contact.point, Quaternion.identity); // Instantiate the particle system at the point of collision
        }
    }
}
