using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;

    // Track targets hit during this specific swing
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    private void OnEnable()
    {
        // Clear history whenever the hitbox turns ON
        hitEnemies.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !hitEnemies.Contains(other))
        {
            SlimeEnemy slime = other.GetComponent<SlimeEnemy>();
            if (slime != null)
            {
                slime.TakeDamage(damageAmount);
                hitEnemies.Add(other);
            }
        }
    }
}