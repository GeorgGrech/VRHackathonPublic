using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public AudioClip bellSound; // The bell sound clip

    private Animator bellAnim;
    private WaveManager waveManager;
    private VillagerManager villagerManager;

    // Start is called before the first frame update
    void Start()
    {
        waveManager = WaveManager._instance;
        villagerManager = VillagerManager._instance;
        bellAnim = GetComponent<Animator>();
    }

    public void RingBell()
    {
        if (waveManager.gameStarted)
        {
            bellAnim.SetTrigger("Ring");
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);
            Debug.Log("---------------- Ringing bell --------------");
            villagerManager.bellRung = true;
            villagerManager.CallVillagers();


        }
        else
        {
            Debug.Log("Cannot ring bell. Game hasn't started yet");
        }
    }
}
