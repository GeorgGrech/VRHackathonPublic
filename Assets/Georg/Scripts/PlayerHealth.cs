using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(Death());
        }
    }

    public IEnumerator Death()
    {
        Debug.Log("Player killed");
        StartCoroutine(MessageManager._instance.DisplayMessages(0, new List<(string, float)>
                { ("You've been slain by the enemy! You lose!",10)}));
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    [ContextMenu("Kill Player")]
    public void TestDeath()
    {
        StartCoroutine(Death());
    }
    
}
