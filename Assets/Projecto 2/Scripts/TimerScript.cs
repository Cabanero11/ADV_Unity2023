using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TextMeshPro timerText;
    public static float remainingTime = 30f;
    private bool hasClicked = false;
    private bool timeStop = false;
    static public bool hasStoped = false;
    private GameObject crateSpawner;
    private GameObject controles;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        hasClicked = false;
        timeStop = false;
        hasStoped = false;
        timerText.text = remainingTime.ToString();
        crateSpawner = GameObject.Find("CrateSpawner");
        controles = GameObject.Find("Controles");
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasClicked)
        {
            StartTimer();
            hasClicked = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(hasClicked && !timeStop)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();

            if (remainingTime <= 0f) // SI YA ACABARON LOS 30S
            {
                hasStoped = true;
                StopTimer();

                crateSpawner.SetActive(false);
                //DisableMouseControls();
            }

            if(remainingTime <= 22f)
            {
                controles.SetActive(false);
            }
        }  
    }

    void StartTimer()
    {
        timerText.text = remainingTime.ToString();
    }

    void StopTimer()
    {
        timeStop = true;
        timerText.text = "0";
    }
    void DisableMouseControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
