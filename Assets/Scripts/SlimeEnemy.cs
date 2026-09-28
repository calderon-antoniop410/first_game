using UnityEngine;
using UnityEngine.AI;

public class SlimeEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;

    [Header("Attack Settings")]
    [SerializeField] private float deathAnimationLength = 0.6f;
    private bool isDead = false;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float repathInterval = 0.2f;

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

            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
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
  
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
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
