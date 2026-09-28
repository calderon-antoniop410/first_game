using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Death & Scene Transition")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private float returnToMenuDelay = 5f;

    [Header("UI Reference")]
    [SerializeField] private Slider healthSlider;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        // Check fatal damage first so "Hurt" doesn't cancel the death sequence
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
    }

    private void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Force transition directly into the death animation state
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.Play("Player_Death");
        }

        // Disable control and physics while keeping the GameObject active for the coroutine
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        StartCoroutine(ReturnToMainMenuRoutine());
    }

    // Waits in real time so timeScale pauses don't freeze the scene transition
    private IEnumerator ReturnToMainMenuRoutine()
    {
        yield return new WaitForSecondsRealtime(returnToMenuDelay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}