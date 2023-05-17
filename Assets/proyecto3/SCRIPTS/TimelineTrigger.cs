using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector timeline; // Assign your timeline asset to this in the inspector

    private bool hasPlayerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayerEntered)
        {
            hasPlayerEntered = true;
            timeline.Play();
            Debug.Log("Outro Played");
        }
    }
}
