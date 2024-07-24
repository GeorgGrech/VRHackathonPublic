using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();

            //if scaleWalls was removed from here. This fixes default scalePoints climbing, but means if enemies accidentally touch scalePoint they climb regardless
            Debug.Log("Scaling walls");
            //StartCoroutine(enemyScript.ScaleWall(transform.Find("ClimbPoint1"), transform.Find("ClimbPoint2"))); //Pass ClimbPoints as parameters
            enemyScript.StartScaleWall(transform.Find("ClimbPoint1"), transform.Find("ClimbPoint2")); //Pass ClimbPoints as parameters

        }
    }
}
