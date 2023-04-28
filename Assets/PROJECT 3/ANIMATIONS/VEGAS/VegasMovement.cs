using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegasMovement : MonoBehaviour
{
    [Header("Variables")]

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public Transform cam;

    private float h, v;
    private Animator animator;
    private CharacterController characterController;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        characterController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0f, v).normalized;


        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
            //animator.SetFloat("H", h);
            //animator.SetFloat("V", v);
        }

        
    }
}
