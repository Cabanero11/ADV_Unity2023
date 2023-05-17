using UnityEngine;


public class AudioTrigger : MonoBehaviour
{
    public string playOnTriggerTag = "PlayAudio";
    public string stopOnTriggerTag = "StopAudio";
    //public AudioClip audioClip;

    public AudioSource audioSource;

    private void Start()
    {
        //audioSource.clip = audioClip;
        audioSource.loop = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playOnTriggerTag))
        {
            audioSource.Play();
        }
        else if (other.CompareTag(stopOnTriggerTag))
        {
            audioSource.Stop();
        }
    }
}
