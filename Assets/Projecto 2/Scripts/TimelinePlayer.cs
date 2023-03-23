using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    PlayableDirector timeline;

    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
    }


    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            PlayTimeline();
        }
    }

    public void PlayTimeline()
    {
        timeline.Play();
    }
}