using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemy : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
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
                ShootEnemy();
                timetoShoot = originalTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
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
                GameObject currentArrow = Instantiate(arrow, ShootPoint.position, ShootPoint.rotation);
                Rigidbody rig = currentArrow.GetComponent<Rigidbody>();

                rig.AddForce((closestTarget.transform.position - ShootPoint.position).normalized * arrowSpeed, ForceMode.VelocityChange);

                ArrowScript arrowScript = currentArrow.GetComponent<ArrowScript>();
                arrowScript.damageAmount = damageAmount;
            }
        }
    }
}

