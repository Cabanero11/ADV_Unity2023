using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneOnSpace : MonoBehaviour
{
    public string sceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TimerScript.remainingTime = 30f;
            SceneManager.LoadScene(sceneName);
        }
    }
}
