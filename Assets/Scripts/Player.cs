using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float horizontal;
    private static float speed = 7.7f;
    private float sprintingSpeed = speed + speed / 2;

    public float jumpingPower = 21f;

    private bool isFacingRight = true;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundCheck groundChecker;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    float jumpPressedRemember = 0.0f;
    float jumpPressedRememberTime = 0.125f;

    float groundedRemember = 0.0f;
    float groundedRememberTime = 0.125f;

    public Gamepad gamepad;

    private void Awake()
    {
        gamepad = new Gamepad();

        gamepad.Game.Jump.performed += ctx => JumpGamepad();

        gamepad.Game.Sprint.performed += ctx => SprintGamepad();
        gamepad.Game.Sprint.canceled += ctx => SprintCanceledGamepad();
    }

    private void OnEnable()
    {
        gamepad.Game.Enable();
    }

    private void OnDisable()
    {
        gamepad.Game.Disable();
    }


    void JumpGamepad()
    {
        jumpPressedRemember -= Time.deltaTime;
        groundedRemember -= Time.deltaTime;
        

        if (groundChecker.isGrounded)
        {
            jumpPressedRemember = jumpPressedRememberTime;
            groundedRemember = groundedRememberTime;
        }

        if (jumpPressedRemember > 0.0f && groundedRemember > 0.0f)
        {
            jumpPressedRemember = 0.0f;
            groundedRemember = 0.0f;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
    }

    void JumpKeyboard()
    {
        jumpPressedRemember -= Time.deltaTime;
        groundedRemember -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }

        if (groundChecker.isGrounded)
        {
            groundedRemember = groundedRememberTime;
        }

        if (jumpPressedRemember > 0.0f && groundedRemember > 0.0f)
        {
            jumpPressedRemember = 0.0f;
            groundedRemember = 0.0f;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
    }

    void SprintKeyboard()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintingSpeed;
        }

        else
        {
            speed = 7.7f;
        }
    }

    void SprintGamepad()
    {
        speed = sprintingSpeed;
    }

    void SprintCanceledGamepad()
    {
        speed = 7.7f;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        JumpKeyboard();
        SprintKeyboard();

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
}