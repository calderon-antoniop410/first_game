using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerAudio playerAudio;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Reference the PlayerAudio component on this GameObject
        playerAudio = GetComponent<PlayerAudio>();
    }

    private void Update()
    {
        // Check for Spacebar OR Left Click
        bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool leftClickPressed = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (spacePressed || leftClickPressed)
        {
            TriggerAttack();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            TriggerAttack();
        }
    }

    private void TriggerAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Call the separate audio script
        if (playerAudio != null)
        {
            playerAudio.PlaySwordSwing();
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // Movement
        rb.linearVelocity = moveInput * moveSpeed;

        // Animator Speed parameter
        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput.sqrMagnitude);
        }

        // Direction flipping
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}