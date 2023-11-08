using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;

    void Update()
    {
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}