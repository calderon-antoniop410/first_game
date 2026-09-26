using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject swordHitbox;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float defaultXOffset = 0.1f;

    private bool isAttacking = false;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerAudio playerAudio;

    private float hitboxXOffset;
    private int facingDirection = 1; // 1 = Right, -1 = Left (Defaults to facing right)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAudio = GetComponent<PlayerAudio>();

        if (swordHitbox != null)
        {
            float currentX = Mathf.Abs(swordHitbox.transform.localPosition.x);
            hitboxXOffset = (currentX > 0.01f) ? currentX : defaultXOffset;
            swordHitbox.SetActive(false);
        }
    }

    private void Update()
    {
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
        if (isAttacking) return;

        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (playerAudio != null)
        {
            playerAudio.PlaySwordSwing();
        }

        if (swordHitbox != null)
        {
            // Position the hitbox based on current facing direction right before activating
            FlipHitbox(facingDirection);
            StartCoroutine(ActivateHitbox());
        }
    }

    private IEnumerator ActivateHitbox()
    {
        swordHitbox.SetActive(true);
        yield return new WaitForSeconds(attackCooldown);
        swordHitbox.SetActive(false);

        isAttacking = false;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput.sqrMagnitude);
        }

        // Update facing direction when actively moving
        if (moveInput.x > 0)
        {
            facingDirection = 1;
            spriteRenderer.flipX = false;
            FlipHitbox(facingDirection);
        }
        else if (moveInput.x < 0)
        {
            facingDirection = -1;
            spriteRenderer.flipX = true;
            FlipHitbox(facingDirection);
        }
    }

    private void FlipHitbox(int direction)
    {
        if (swordHitbox != null)
        {
            Vector3 currentPos = swordHitbox.transform.localPosition;
            swordHitbox.transform.localPosition = new Vector3(hitboxXOffset * direction, currentPos.y, currentPos.z);
        }
    }
}