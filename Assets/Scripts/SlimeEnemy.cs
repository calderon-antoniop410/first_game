using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlimeEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;

    [Header("Attack Settings")]
    [SerializeField] private float deathAnimationLength = 0.6f;

    private bool wasInRange;
    private bool isDead = false;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float repathInterval = 0.2f;

    [Header("Visual Feedback")]
    [SerializeField] private Color hitFlashColor = new Color(1f, 0.3f, 0.3f, 1f);
    [SerializeField] private float flashDuration = 0.12f;
    private Color baseColor;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    private NavMeshAgent agent;
    private SlimeSpawner spawner;
    private float nextAttackTime = 0f;
    private float nextRepathTime = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            baseColor = spriteRenderer.color;
        }

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null || isDead) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= attackRange)
        {
            agent.isStopped = true;

            if (!wasInRange)
            {
                wasInRange = true;
                nextAttackTime = Time.time + attackCooldown;
            }

            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            wasInRange = false;
            agent.isStopped = false;

            if (Time.time >= nextRepathTime)
            {
                agent.SetDestination(playerTransform.position);
                nextRepathTime = Time.time + repathInterval;
            }
        }

        FlipSprite(agent.velocity.x);

        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        }
    }

    private void FlipSprite(float directionX)
    {
        if (Mathf.Abs(directionX) < 0.1f) return;
        spriteRenderer.flipX = directionX > 0;
    }

    private void AttackPlayer()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;

        nextAttackTime = Time.time + attackCooldown; // Reset attack cooldown when taking damage
        if (spriteRenderer != null && gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(HitFlashRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
    }

    private IEnumerator HitFlashRoutine()
    {
        spriteRenderer.color = hitFlashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = baseColor;
    }

    public void SetSpawner(SlimeSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void Die()
    {
        isDead = true;
        agent.isStopped = true;
        agent.enabled = false;

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        if (spawner != null)
        {
            spawner.OnSlimeDied();
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddKill();
        }

        Destroy(gameObject, deathAnimationLength);
    }
}