 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Settings")]
    public string horizontalInput = "Horizontal_P1";
    public string jumpInput = "Jump_P1";

    [Header("Movement")]
    public float moveSpeed = 12f;
    public float acceleration = 40f;
    public float deceleration = 60f;
    public float airControlMultiplier = 0.6f;
    public bool canMove = true; // Flag to enable/disable movement

    [Header("Jumping")]
    public float jumpForce = 10f;
    public float coyoteTime = 0.1f; // Hang time after leaving ground
    public float jumpBufferTime = 0.1f; // Input buffer for jump

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove) return; // Check if movement is enabled

        // Input
        moveInput = Input.GetAxisRaw(horizontalInput);

        // Jump input buffering
        if (Input.GetButtonDown(jumpInput))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        // Jumping
        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0;
        }

        // Better jump feel
        if (rb.velocity.y < 0)
        {
            // Falling
            rb.gravityScale = 3f; // Increase this value to fall faster
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Jump cut (released button mid-air)
            rb.gravityScale = 2f;
        }
        else
        {
            // Normal jump
            rb.gravityScale = 1f;
        }

        if(moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); //right
        } else if (moveInput < 0){
            transform.localScale = new Vector3(-1, 1, 1); //left
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = isGrounded ? (Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration) : acceleration * airControlMultiplier;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, 0.9f) * Mathf.Sign(speedDif);

        rb.AddForce(Vector2.right * movement);

        // Limit max horizontal speed
        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * moveSpeed, rb.velocity.y);

        animator.SetFloat("xVelocityAbs", Mathf.Abs(rb.velocity.x));
        animator.SetBool("onGround", isGrounded);
    }

    public void DisableMovement()
    {   
        canMove = false;
        rb.velocity = Vector2.zero; // Stop all movement
        rb.isKinematic = true;
        
    }

    public void EnableMovement()
    {
        canMove = true;
        rb.isKinematic = false;
    }
}

