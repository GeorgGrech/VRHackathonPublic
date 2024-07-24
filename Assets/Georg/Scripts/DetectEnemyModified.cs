using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemyModified : MonoBehaviour
{
    bool isDetected;
    List<GameObject> targets;
    public Transform archerAI;
    Quaternion initialRotation;

    public GameObject arrow;
    public Transform ShootPoint;

    public float arrowSpeed = 10f;
    public float timetoShoot = 1.3f;
    float originalTime;

    public float detectionRadius = 10f;
    public int damageAmount = 20;

    private Animator animator;
    public string animName;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();
        originalTime = timetoShoot;
        initialRotation = archerAI.rotation;
        targets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDetected && targets.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestTarget = null;

            foreach (GameObject enemy in targets)
            {
                if (enemy != null) // Skip null targets
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = enemy;
                    }
                }
            }

            if (closestTarget != null)
            {
                archerAI.LookAt(closestTarget.transform);
            }
        }
        else
        {
            // Reset the rotation to the initial rotation when not detecting an enemy
            archerAI.rotation = initialRotation;
        }
    }

    private void FixedUpdate()
    {
        if (isDetected)
        {
            timetoShoot -= Time.deltaTime;

            if (timetoShoot < 0)
            {
                if (animName != "shoot")
                {
                    animName = "shoot";
                    animator.SetTrigger("shoot");
                    animator.SetBool("isShooting",true);
                }
                ShootEnemy();
                timetoShoot = originalTime;
            }
        }
        else
        {
            if (animName != "Idle")
            {
                animName = "Idle";
                animator.SetTrigger("Idle");
                animator.SetBool("isShooting", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy OnTrigger");
            targets.Add(other.gameObject);
            isDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targets.Remove(other.gameObject);

            if (targets.Count == 0)
            {
                isDetected = false;
            }
        }
    }

    private void ShootEnemy()
    {
        if (targets.Count > 0)
        {
            Debug.Log("Shoot Enemy method");
            GameObject closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject enemy in targets)
            {
                if (enemy == null)
                    continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = enemy;
                }
            }

            if (closestTarget != null)
            {
                Debug.Log("Shooting enemy (Arrow instantiate)");
                GameObject currentArrow = Instantiate(arrow, ShootPoint.position, ShootPoint.rotation);
                Rigidbody rig = currentArrow.GetComponent<Rigidbody>();

                rig.AddForce((closestTarget.transform.position - ShootPoint.position).normalized * arrowSpeed, ForceMode.VelocityChange);

                ArrowScriptModified arrowScript = currentArrow.GetComponent<ArrowScriptModified>();
                arrowScript.damageAmount = damageAmount;
            }
        }
    }
}

