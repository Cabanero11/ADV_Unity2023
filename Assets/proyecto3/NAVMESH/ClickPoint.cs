using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class ClickPoint : MonoBehaviour
{
    [System.Serializable]
    public class AgentType
    {

    }


    public List<NavMeshAgent> agents;
    private Camera mainCamera;
    public static Vector3 targetPosition;

    public GameObject target;
    public GameObject RealTarget;


    void Start()
    {
        mainCamera = Camera.main;
        RealTarget = Instantiate(target, transform.position, Quaternion.identity);
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                foreach (NavMeshAgent agent in agents)
                {
                    agent.SetDestination(hit.point);
                    RealTarget.transform.position = targetPosition = hit.point;
                }
            }
        }
    }
}