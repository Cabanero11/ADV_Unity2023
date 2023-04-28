using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlChest : MonoBehaviour
{

    public Animator animator;
   
    void Start()
    {
        
    }

    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            //Debug.Log("Collision");
            bool state = animator.GetBool("Open");
            animator.SetBool("Open", !state);
        } 
    }
}
