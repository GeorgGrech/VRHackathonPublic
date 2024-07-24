using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private WaveManager waveManager;
    private VillagerManager villagerManager;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        waveManager = WaveManager._instance;
        villagerManager = VillagerManager._instance;

        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Opengate()
    {
        if (waveManager.gameStarted && !villagerManager.villagersCalled) //If enemies have been detected and villagers arent already safe inside
        {
            Debug.Log("Opening gate");
            animator.Play("GateOpen");
            villagerManager.gateOpened = true;
            villagerManager.CallVillagers();
        }
        else
        {
            Debug.Log("Cannot open gate. Game hasn't started yet");
        }
    }

    private void OnMouseDown() //Temporary method, just for testing
    {
        Opengate();
    }

    public void CloseGate()
    {
        animator.Play("GateClose");
        villagerManager.gateOpened = false;
    }
}
