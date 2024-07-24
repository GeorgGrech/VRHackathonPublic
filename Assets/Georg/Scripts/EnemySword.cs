using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (enemy.animName == "Attack") //Only damage if in attack mode
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(enemy.swordDamage);
            }
            if (other.CompareTag("Villager"))
            {
                other.gameObject.GetComponent<Villager>().TakeDamage(enemy.swordDamage);
            }
        }
    }
}
