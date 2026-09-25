using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has a specific tag (e.g., "Enemy" or "Breakable")
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy: " + other.name);
            // Add damage or destruction logic here later
        }
    }
}