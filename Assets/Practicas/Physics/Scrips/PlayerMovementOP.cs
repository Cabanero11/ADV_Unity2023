using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOP : MonoBehaviour
{
    // SI EL JUGADOR SE QUEDA FRENADO EN PAREDES, se le pone un Physics2D.material con TODO a 0
    
        
        
    #region Variables Y Explicacion
    [Header("Variables Movimiento")]
    public float speed;
    public float jumpForce;
    private float moveInput;
    private float moveInput2;
    public string axisHorizontalName;   // El nombre de los input horizontales
    public string axisVerticalName;     // Si quieres que se mueva verticalmente
    private bool facingRight = true;
    public bool wantToFlip;             // Si quieres que se gire ponlo a true, sino quieres que gire ponloa a false

    [Space]
                                        // En vez deRaycast Usa un Circle Collider con Trigger
                                        // Necesitas un GameObject (Llamado GroundCheck mismo), le pones algo para que se vea (Un Gizmos se coloca en sus pies) , y lo haces hijo del jugador
    [Header("Variables Salto")]
    public bool isGrounded;             // Para comprobar si esta tocando o no
    public Transform groundCheck;       // Aqui se arrastra el GameObject creado para ver si Toca el suelo
    public float checkRadius;           // Algo de radio al circulo !OJO¡ --> checkRadius > 0
    public LayerMask whatIsGround;      // Se pone la Tag que tengan los suelos, Ground va bien
    private int extraJumps;             // Si vale 2 --> Tiene Doble Salto, si vale 3 --> Tiene Triple Salto
    public int extraJumpsValue;         // Ponle mas de 1 si quieres, Doble Tiple Salto etc...


    [Space]

    [Header("Componentes")]
    public Rigidbody rb;
    private CapsuleCollider col;       // Coge el collider del circulo para ver si toca el suelo con él, usalo si no quieres usar esto --> isGrounded = Physics2D.IsTouchingLayers(col, whatIsGround);isGrounded = Physics2D.IsTouchingLayers(col, whatIsGround);
    #endregion





    // Inicializar Cosas
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Para el movimiento
    void FixedUpdate()
    {
        //isGrounded = Physics2D.IsTouchingLayers(col, whatIsGround); // Otra manera si no quieres CircleCollider2D y el CheckGround
        //isGrounded = Physics.OverlapCapsule(groundCheck.position, groundCheck, whatIsGround);

        moveInput = Input.GetAxisRaw(axisHorizontalName);
        moveInput2 = Input.GetAxisRaw(axisVerticalName);

        rb.velocity = new Vector2(moveInput * speed, moveInput2 * speed); // Pon esto si NO VERTICAL  rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Para Voltear al personaje
        if (facingRight == false && moveInput > 0) // Para la derecha
        {
            Flip();
        }
        else if(facingRight == true && moveInput < 0) // Para la izquierda
        {
            Flip();
        }
    }

    // Para el Salto
    void Update()
    {
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumpsValue > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumpsValue -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumpsValue == 0)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    // Se puede usar tambien -->  SpriteRenderer.flipX = True/False;﻿
    void Flip()
    {
        if (wantToFlip)
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;  // El LocalScale es un Vector3
            Scaler.x *= -1;                         // Da la vuelta
            transform.localScale = Scaler;          // El transform es el player
        }
    }
}