using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float horizontal;

    [SerializeField]
    public static float defaultSpeed = 7.7f;
    private static float speed = defaultSpeed;
    private float sprintingSpeed = speed + speed / 2;

    public float jumpingPower = 21f;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundCheck groundChecker;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    float jumpPressedRemember = 0.0f;
    float jumpPressedRememberTime = 0.5f;

    float groundedRemember = 0.0f;
    float groundedRememberTime = 0.5f;

    private bool isFacingRight = true;


    bool isSprinting = false;
    bool isJumping = false;

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

    public void ResetToSpawnPosition()
    {
        gameObject.transform.position = spawnPosition;
    }
}