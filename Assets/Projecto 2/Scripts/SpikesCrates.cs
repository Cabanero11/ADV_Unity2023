using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesCrates : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Spikes")){
            Destroy(gameObject);
        }
    }
}
