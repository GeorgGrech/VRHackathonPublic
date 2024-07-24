using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int damageAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(2f));
    }

    private System.Collections.IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
