using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundCheck groundChecker;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private WallCheck wallChecker;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform frontCheck;

    [SerializeField] private float wallCheckDistance;
    [SerializeField] public static float defaultSpeed = 8.2f;
    [SerializeField] public float wallSlidingSpeed = 2.0f;


    private static float speed = defaultSpeed;
    private float sprintingSpeed = speed + speed / 2;

    public Vector2 checkSize;

    private float horizontal;

    public float jumpingPower = 21f;

    float jumpPressedRemember = 0.0f;
    float jumpPressedRememberTime = 0.5f;

    float groundedRemember = 0.0f;
    float groundedRememberTime = 0.5f;

    private bool isFacingRight = true;


    bool isSprinting = false;
    bool isJumping = false;
    bool isTouchingFront;
    bool isTouchingWall;
    bool isWallSliding;

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

        jumpPressedRemember -= Time.deltaTime;
        groundedRemember -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (isSprinting)
        {
            speed = sprintingSpeed;
        }

        else
        {
            speed = defaultSpeed;
        }

        if (isJumping && groundChecker.isGrounded)
        {
            jumpPressedRemember = jumpPressedRememberTime;
            groundedRemember = groundedRememberTime;

            if (jumpPressedRemember > 0.0f && groundedRemember > 0.0f)
            {
                FindObjectOfType<AudioManager>().Play("PlayerJump");
                jumpPressedRemember = 0.0f;
                groundedRemember = 0.0f;
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }
        }

        else
        {
            isJumping = false;
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

    private void CheckWallSliding()
    {
        if (wallChecker.isTouchingAWall && !groundChecker.isGrounded && rb.velocity.y < 0)
        {
            wallChecker.isWallSliding = true;
        }

        else
        {
            wallChecker.isWallSliding = false;
        }
    }

    public void ResetToSpawnPosition()
    {
        gameObject.transform.position = spawnPosition;
    }
}