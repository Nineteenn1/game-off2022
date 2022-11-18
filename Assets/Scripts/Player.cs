using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundCheck groundChecker;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    [SerializeField] public static float defaultSpeed = 8.2f;
    [SerializeField] public float wallSlidingSpeed = 2.0f;


    private static float speed = defaultSpeed;
    private float sprintingSpeed = speed + speed / 2;


    private float horizontal;

    public static float jumpingPower = 21f;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferTimeCounter = 0.0f;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter = 0.0f;

    private bool isFacingRight = true;


    bool isSprinting = false;
    public bool isJumping = false;

    bool isWallJumping;
    bool isWallSliding;

    float wallJumpingDirection;
    float wallJumpingTime;
    float wallJumpingTimeCounter;
    float wallJumpingDuration;

    Vector2 wallJumpingPower = new Vector2(defaultSpeed, jumpingPower);

    public Gamepad gamepad;

    Vector3 spawnPosition;

    private void Awake()
    {
        spawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        gamepad = new Gamepad();

        gamepad.Game.Jump.performed += ctx => Jump();
        gamepad.Game.Sprint.performed += ctx => Sprint();
        gamepad.Game.Sprint.canceled += ctx => CancelSprint();

        gamepad.Game.SprintKeyboard.performed += ctx => Sprint();
        gamepad.Game.SprintKeyboard.canceled += ctx => CancelSprint();

    }

    private void OnEnable()
    {
        gamepad.Game.Enable();
    }

    private void OnDisable()
    {
        gamepad.Game.Disable();
    }

    void Jump()
    {
        isJumping = true;
    }

    void Sprint()
    {
        isSprinting = true;
    }

    void CancelSprint()
    {
        isSprinting = false;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (groundChecker.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }

        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (isJumping)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }

        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0.0f && jumpBufferTimeCounter > 0.0f)
        {
            FindObjectOfType<AudioManager>().Play("PlayerJump");

            jumpBufferTimeCounter = 0.0f;
            coyoteTimeCounter = 0.0f;

            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
     

        else
        {
            isJumping = false;
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (isSprinting)
        {
            speed = sprintingSpeed;
        }

        else
        {
            speed = defaultSpeed;
        }
    }


    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (isWalled() && !groundChecker.isGrounded /*&& horizontal != 0f*/)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.x, -wallSlidingSpeed, float.MaxValue));;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingTimeCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingTimeCounter -= Time.deltaTime;
        }

        if (isJumping && wallJumpingTimeCounter > 0.0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingTimeCounter = 0.0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector2 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    public void ResetToSpawnPosition()
    {
        gameObject.transform.position = spawnPosition;
    }
}