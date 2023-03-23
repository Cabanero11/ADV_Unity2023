using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController2 : MonoBehaviour
{
    private Camera cam;
    private bool pressingMouse = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            pressingMouse = true;
            Debug.Log("true");
        }

        if (Input.GetMouseButtonUp(0)) // release the left click
        {
            pressingMouse = false;
            Debug.Log("false");
        }

        //print(pressingMouse);

        if(pressingMouse)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("moviendo");

            var lookPos = mousePos - transform.position;
            lookPos.z = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 6);
            transform.LookAt(mousePos);

        }
    }
}
