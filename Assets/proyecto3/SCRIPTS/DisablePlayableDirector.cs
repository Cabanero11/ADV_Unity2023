using UnityEngine;
using UnityEngine.Playables;

public class DisablePlayableDirector : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    private PlayableDirector playableDirector;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.stopped += OnTimelineFinished;
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
            playableDirector.stopped -= OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
