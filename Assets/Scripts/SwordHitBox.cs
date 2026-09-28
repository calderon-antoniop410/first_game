using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;

    // Track enemies hit during this specific swing
    private HashSet<SlimeEnemy> hitEnemies = new HashSet<SlimeEnemy>();

    private void OnEnable()
    {
        // Clear history whenever the hitbox turns ON
        hitEnemies.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SlimeEnemy slime = other.GetComponentInParent<SlimeEnemy>();
        if (slime != null && !hitEnemies.Contains(slime))
        {
            slime.TakeDamage(damageAmount);
            hitEnemies.Add(slime);
        }
    }
}