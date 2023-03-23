using UnityEngine;
using UnityEngine.Playables;

public class TimeLineOutro : MonoBehaviour
{
    PlayableDirector timelineOutro;

    void Start()
    {
        timelineOutro = GetComponent<PlayableDirector>();
    }


    private void Update()
    {
        if (TimerScript.hasStoped == true)
        {
            PlayTimeline();
            TimerScript.hasStoped = false;
        }
    }

    public void PlayTimeline()
    {
        timelineOutro.Play();
    }
}