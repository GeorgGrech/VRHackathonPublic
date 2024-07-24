using UnityEngine;

public class ArrowScriptModified : MonoBehaviour
{
    public int damageAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();

            if (enemyScript != null) // Can't say I fully understand why this is here, but I won't mess with your artistic decisions, Ander
            {
                enemyScript.TakeDamage(damageAmount);
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
