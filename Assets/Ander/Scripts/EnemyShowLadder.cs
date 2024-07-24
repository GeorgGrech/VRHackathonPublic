using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShowLadder : MonoBehaviour
{
    public GameObject prefabContainer; // Reference to the GameObject containing the prefabs

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            prefabContainer.SetActive(true); // Show the prefab container GameObject
        }
    }
}
