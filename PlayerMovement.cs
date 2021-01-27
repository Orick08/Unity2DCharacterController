using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float speed;
    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;

    private float moveInput;
    private bool facingRight = true;

    private bool isGrounded = true;
    [Header("Ground properties")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //Jump
    public int extraJumps;
    private int currentExtraJump;
    private bool isJumping = false;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentExtraJump = extraJumps;
    }

    private void Update()
    {
        if(isGrounded == true)
        {
            if(currentExtraJump <= 0)
                currentExtraJump = extraJumps;

            animator.SetBool("onair", false);
        }
        else
        {
            animator.SetBool("onair", true);
        }

        if(currentExtraJump > 0 && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            currentExtraJump--;
        }

        if(Input.GetButton("Jump") && isJumping)
        {
            if(jumpTimeCounter > 0)
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
        //Check if is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);  

        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));

        if(facingRight == false && moveInput > 0)
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
}
