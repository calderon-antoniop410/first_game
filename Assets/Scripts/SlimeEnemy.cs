using System.Diagnostics;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 30; // Slime's total health
    private int currentHealth;

    [Header("Attack Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private float attackCooldown = 1.5f;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private float nextAttackTime = 0f;

    private void Start()
    {
        currentHealth = maxHealth; // Set health at start
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }

    private void AttackPlayer()
    {
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            UnityEngine.Debug.Log("Slime attacked the player for " + attackDamage + " damage!");
        }
    }

    // Call this function when the player hits the slime!
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UnityEngine.Debug.Log("Slime took " + damage + " damage! Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        UnityEngine.Debug.Log("Slime defeated!");
        Destroy(gameObject); // Removes the slime from the game
    }
}