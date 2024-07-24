using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHorde : MonoBehaviour
{
    [SerializeField] private int lifetime;
    [SerializeField] private float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (movementSpeed * Time.deltaTime);
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
