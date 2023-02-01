using UnityEngine;
using UnityEditor;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float speed;
    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;

    private float moveInput;
    private bool facingRight = true;
    public LayerMask whatIsGround;

    //Jump
    public int jumpsCount;
    private int currentJumpsCount;
    private bool isJumping = false;

    // This game mecanics
    private bool foward = false;
    private float forceMoveInput = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentJumpsCount = jumpsCount;
    }

    private void Update()
    {
        if (IsGrounded() && !isJumping)
        {
            if (currentJumpsCount <= 0)
            {
                currentJumpsCount = jumpsCount;
            }


            animator.SetBool("onair", false);
        }
        else
        {
            animator.SetBool("onair", true);
        }

        if (currentJumpsCount > 0 && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            currentJumpsCount--;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

    }

    private void FixedUpdate()
    {
        if (foward)
        {
            moveInput = forceMoveInput;
        }
        else
        {
            moveInput = Input.GetAxisRaw("Horizontal");

        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, whatIsGround);
    }

    public void forceFoward(bool arg)
    {
        foward = arg;
    }
}