using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5;
    [SerializeField] float climbSpeed = 5;

    NavMeshAgent navAgent;
    NavMeshObstacle navObstacle;

    public WaveManager waveManager;

    public List<GameObject> potentialTargets;
    public Transform currentTarget;

    public bool destinationReached = false;

    public bool scaleWalls = false;
    public bool wallScaled = false;

    //public Coroutine scalingCoroutine;


    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private Animator animator;
    public string animName;

    public bool dead = false;

    public int swordDamage = 10;

    public Collider[] ragdollColliders;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetTrigger("Run");
        animName = "Run";

        currentHealth = maxHealth;

        navAgent = gameObject.GetComponent<NavMeshAgent>();
        navObstacle = gameObject.GetComponent<NavMeshObstacle>();

        potentialTargets = GameObject.FindGameObjectsWithTag("Villager").ToList<GameObject>(); //Add villagers to target
        potentialTargets.Add(GameObject.FindGameObjectWithTag("Player")); //Add player to list of targets

        currentTarget = potentialTargets[0].transform; //Set to a target on start just to not be null

    }

    // Update is called once per frame
    void Update()
    {
        if (!scaleWalls && !dead)
        {
            CheckForClosestTarget(); //If selected to scale walls, ignore closest target
            CheckDestinationReached();
            CheckDestinationPossible();
        }


        if (navAgent.isActiveAndEnabled)
            MoveToTarget();



        /*if (destinationReached)
        {
            RotateToPlayer();
        }*/
    }

    private void CheckForClosestTarget()
    {
        //Vector3 currentTargetPos = navAgent.destination; //Start with current destination
        float currentTargetDistance = Vector3.Distance(transform.position, navAgent.destination);

        foreach (GameObject target in potentialTargets)
        {
            if (target != null)
            {
                float newTargetDistance = Vector3.Distance(transform.position, target.transform.position);

                if (newTargetDistance < currentTargetDistance) //If distance smaller than what has been found
                {
                    currentTargetDistance = newTargetDistance;
                    currentTarget = target.transform;
                }
            }
        }
    }

    private void MoveToTarget()
    {
        if (currentTarget != null)
        {
            navAgent.destination = currentTarget.position;
        }
    }


    void RotateToPlayer()
    {
        //float rotationSpeed = baseRotationSpeed * gameManager.difficultyModifier; //multiply base rotation speed by the difficulty ratio


        Vector3 TargetPosition = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);

        transform.rotation = Quaternion.RotateTowards(
        transform.rotation,
        Quaternion.LookRotation(TargetPosition - transform.position),
        Time.deltaTime * rotationSpeed);
    }

    public void CheckDestinationReached()
    {
        if (currentTarget != null && currentTarget.gameObject != null)
        {
            if ((currentTarget.position - transform.position).sqrMagnitude < Mathf.Pow(navAgent.stoppingDistance, 2))
            {
                EnableObstacle(true);
                RotateToPlayer();
                if (animName != "Attack")
                {
                    animator.SetBool("isAttacking", true);
                    animName = "Attack";
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                EnableObstacle(false);

                // Check if current target is a dead villager and skip it
                if (currentTarget.CompareTag("Villager") && currentTarget.GetComponent<Villager>().dead)
                {
                    SetNextTarget();
                }
                else
                {
                    if (animName != "Run")
                    {
                        animator.SetBool("isAttacking", false);
                        animName = "Run";
                        animator.SetTrigger("Run");
                    }
                    navAgent.destination = currentTarget.position;
                }
            }
        }
    }

    private void SetNextTarget()
    {
        // Remove dead villager from potentialTargets list
        potentialTargets.Remove(currentTarget.gameObject);

        // Find the next target that is not a dead villager
        foreach (GameObject target in potentialTargets)
        {
            if (target != null && target.CompareTag("Villager") && !target.GetComponent<Villager>().dead)
            {
                currentTarget = target.transform;
                return;
            }
        }

        // If no valid target is found, default to the player target
        currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void CheckDestinationPossible()
    {
        if (!wallScaled && navAgent.isActiveAndEnabled) //Once wall is scaled, ignore this method
        {
            if (navAgent.path.status == NavMeshPathStatus.PathPartial)
            {

                if (!currentTarget.name.Contains("ScalePoint"))
                {
                    Debug.Log("Villagers/Player cannot be reached, defaulting to ScalePoints");
                    Transform[] scalePoints = EnemyThreshold._instance.scalePoints;
                    currentTarget = scalePoints[Random.Range(0, scalePoints.Length)];
                }
            }
        }
    }

    public void EnableObstacle(bool enable)
    {
        navObstacle.enabled = enable;
        navAgent.enabled = !enable;
    }

    public void GoToScalePoint(Transform scalePoint)
    {
        scaleWalls = true;
        if (!currentTarget.name.Contains("ScalePoint")) //If already going to scalePoint, dont bother changing
        {
            currentTarget = scalePoint;
        }
    }

    public void StartScaleWall(Transform climbPoint1, Transform climbPoint2) //Called by scalepoint. fixes exception
    {
        StartCoroutine(ScaleWall(climbPoint1, climbPoint2));
    }

    public IEnumerator ScaleWall(Transform climbPoint1, Transform climbPoint2)
    {
        scaleWalls = true;


        navObstacle.enabled = false;
        navAgent.enabled = false;

        potentialTargets.Remove(potentialTargets.Last()); //Remove player from potentialTargets, to focus solely on enemies

        CheckForClosestTarget(); //I shouldn't have to do this but it wasnt working

        //Start climb animation
        while (transform.position != climbPoint1.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, climbPoint1.position, climbSpeed * Time.deltaTime); //Go up until ClimbPoint1 reached
            yield return null;
        }


        //Play Hop over animation
        while (transform.position != climbPoint2.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, climbPoint2.position, climbSpeed * Time.deltaTime); // "Hop Over" to ClimbPoint2
            yield return null;
        }

        EnableObstacle(false);
        scaleWalls = false; //Start tracking again
        wallScaled = true; //Wall has been successfully scaled. Therefore, no longer default to scalePoints
    }

    #region Health and death

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(Death());
        }
    }

    [ContextMenu("Kill Enemy")]
    public IEnumerator Death()
    {
        waveManager.leftInWave--;
        Debug.Log("wavebeingcalled");
        Debug.Log("Enemy killed");


        if (scaleWalls) //if killed when scaling walls, destroy instantly to circumvent errors
        {
            Destroy(gameObject);
        }


        tag = "Untagged"; //This doesn't stop the archers from shooting the corspe, but whatever
        dead = true;

        if (navAgent.enabled)
        {
            navAgent.destination = transform.position;
        }

        navAgent.enabled = false;
        navObstacle.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        EnableRagdollColliders();


        animator.enabled = false;

        //A coroutine can be implemented here, destroying the ragdoll after some time
        yield return new WaitForSeconds(20);

        Destroy(gameObject); //Destroy this gameObject.
    }

    private void OnMouseDown() //Temporary method, just for testing, Kills enemy on click
    {
        StartCoroutine(Death());
    }

    private void EnableRagdollColliders()
    {
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }
    }
    #endregion
}