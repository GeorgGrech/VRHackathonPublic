using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour
{
    [SerializeField] private string[] idleAnims;

    NavMeshAgent navAgent;
    NavMeshObstacle navObstacle;

    public Transform currentTarget;
    public bool called;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private Animator animator;
    private string animName;

    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger(idleAnims[Random.Range(0, idleAnims.Length)]);

        currentHealth = maxHealth;

        navAgent = gameObject.GetComponent<NavMeshAgent>();
        navObstacle = gameObject.GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (called && !dead)
            CheckDestinationReached();
    }

    public void CheckDestinationReached()
    {
        if ((currentTarget.position - transform.position).sqrMagnitude < Mathf.Pow(navAgent.stoppingDistance, 2) + 2)
        { // If destination reached, disable agent and become obstacle
            EnableObstacle(true);
            if (animName != "Crouching")
            {
                animName = "Crouching";
                animator.SetTrigger("Crouching");
            }
        }
        else
        { // If we are not in range, become an agent again
            EnableObstacle(false);
            if (animName != "Run")
            {
                animName = "Run";
                animator.SetTrigger("Run");
            }
            navAgent.destination = currentTarget.position;
        }
    }

    public void EnableObstacle(bool enable)
    {
        navObstacle.enabled = enable;
        navAgent.enabled = !enable;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Villager Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    [ContextMenu("Kill villager")]
    public void Death()
    {
        dead = true;
        Debug.Log("Villager killed");

        /*
        if (navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true; //Stop any tracking
            navAgent.enabled = false; //Disable nav entirely
        }*/
        navAgent.enabled = false;
        navObstacle.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        VillagerManager._instance.VillagerDeath();

        WaveManager._instance.RemoveTarget(gameObject);
        //waveManager.leftInWave--;
        //A coroutine can be implemented here, destroying the ragdoll after some time

        animator.SetTrigger("Dead");

        //Destroy(gameObject); //Destroy this gameObject.

        StartCoroutine(DestroyAfterDelay(200f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

}
