using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    private WaveManager waveManager;
    private VillagerManager villagerManager;

    Animator anim;

    void Start()
    {

        waveManager = WaveManager._instance;
        villagerManager = VillagerManager._instance;

        anim = gameObject.GetComponent<Animator>();
    }

    public void OpenGate()
    {
        if (waveManager.gameStarted) //If enemies have been detected and villagers arent already safe inside
        {
            AudioSource audio = gameObject.GetComponent<AudioSource>();
            audio.PlayOneShot(audio.clip, 1f);

            anim.SetTrigger("OpenGate");

            villagerManager.gateOpened = true;
            villagerManager.CallVillagers();
        }
    }

    public void CloseGate()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip, 1f);

        anim.SetTrigger("CloseGate");
        villagerManager.gateOpened = false;
    }
}
