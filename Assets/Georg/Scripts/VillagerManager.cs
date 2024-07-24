using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class VillagerManager : MonoBehaviour
{
    public static VillagerManager _instance;

    public bool bellRung = false;
    public bool gateOpened = false;

    public bool villagersCalled = false;

    GameObject[] villagers;
    [SerializeField] private Transform[] groupAreas;

    public int villagersLeft;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        villagers = GameObject.FindGameObjectsWithTag("Villager"); //Find all villagers
        villagersLeft = villagers.Length;
    }

    public void CallVillagers()
    {
        if (bellRung && gateOpened && !villagersCalled)
        {
            villagersCalled = true;
            Debug.Log("Calling villagers to fort");
            foreach (GameObject villager in villagers)
            {
                if (villager != null)
                {

                    Villager villagerScript = villager.GetComponent<Villager>();
                    if (!villagerScript.dead)
                    {
                        int randomArea = Random.Range(0, groupAreas.Length); //Choose random spot
                        villagerScript.EnableObstacle(false);
                        villagerScript.currentTarget = groupAreas[randomArea];
                        villager.GetComponent<NavMeshAgent>().destination = groupAreas[randomArea].position; //Set all villagers to go to group area
                        villagerScript.called = true;
                    }
                }
            }
        }
    }

    public void VillagerDeath()
    {
        villagersLeft--;

        if (villagersLeft <= 0)
        {
            StartCoroutine(GameLose());
        }
    }

    public IEnumerator GameLose()
    {
        StartCoroutine(MessageManager._instance.DisplayMessages(0, new List<(string, float)>
                { ("The villagers have all been killed! You lose!",10)}));

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(10);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [ContextMenu("Kill All Villagers")]
    public void KillVillagers()
    {
        foreach (GameObject villager in villagers)
        {
            if (villager != null)
            {
                villager.GetComponent<Villager>().Death();
            }
        }
    }
}
