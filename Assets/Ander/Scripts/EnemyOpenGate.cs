using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOpenGate : MonoBehaviour
{

    private VillagerManager villagerManager;
    public GameObject rampContainer;

    // Start is called before the first frame update
    void Start()
    {
        villagerManager = VillagerManager._instance;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !villagerManager.gateOpened)
        {
            GameObject.Find("GatePrefab").GetComponent<GateScript>().OpenGate();

            rampContainer.SetActive(true); // Show the prefab container GameObject

            StartCoroutine(MessageManager._instance.DisplayMessages(0, new List<(string, float)>
        { ("The enemy have breached our walls! Get in there and defend the villagers!",7)}));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
