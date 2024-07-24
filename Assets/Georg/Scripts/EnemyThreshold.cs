using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThreshold : MonoBehaviour
{
    private VillagerManager villagerManager;


    public Transform[] scalePoints;

    public static EnemyThreshold _instance;
    private void Awake()
    {
        _instance = this; //This is just to simplify CheckDestinationPossible method in enemy
    }

    // Start is called before the first frame update
    void Start()
    {
        villagerManager = VillagerManager._instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!villagerManager.gateOpened) //If gate is open, don't bother. Also make sure to set gateOpened back to false at start of wave 2
            {
                if(Random.value > 0.5f)
                {
                    Debug.Log("Enemy selected as wall scaler");
                    Transform selectedScalePoint = scalePoints[Random.Range(0, scalePoints.Length)]; 
                    other.GetComponent<Enemy>().GoToScalePoint(selectedScalePoint);
                }
                else Debug.Log("Enemy selected to attack player");
            }
        }
    }
}
