using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager _instance;


    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject enemyHorde;
    [SerializeField] private Transform[] hordePoints;

    [SerializeField] private int[] waveValues; //Length of wave values (set from Inspector) decides total amount of waves
    public int leftInWave;

    [SerializeField] private float spawnDelay = 2;

    [SerializeField] private int initialWaitMin;
    [SerializeField] private int initialWaitMax;

    [SerializeField] private int endWaveWaitMin;
    [SerializeField] private int endWaveWaitMax;

    [SerializeField] private int precursorTime; //Static time of precursor animation before enemies appear

    public bool gameStarted = false; //Set permanently to true after initial enemy sighting. Will be used to enable/disable bell and door

    private List<GameObject> enemies;
    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        StartCoroutine(WaveCycle());
        StartCoroutine(MessageManager._instance.DisplayMessages(3, new List<(string, float)>
        { ("Mdina, 1429. Malta is under occupation by the Kingdom of Sicily.",7),
        ("There have been reports of Tunisian invaders on the shore. Keep an eye out for their appearance.",7),
        ("When they appear, sound the alarm bell and open the gates to let the villagers inside.",7),}));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaveCycle()
    {
        int wait = Random.Range(initialWaitMin, initialWaitMax);
        Debug.Log("First wave starting in " + wait + " seconds");
        yield return new WaitForSeconds(wait);

        for (int i = 0; i < waveValues.Length; i++)
        {
            enemies = new List<GameObject>();
            if (i == 0)
            {
                Debug.Log("Here they come! (Animation is playing)");

                foreach(Transform hordePoint in hordePoints)
                {
                    Instantiate(enemyHorde, hordePoint.position, hordePoint.rotation);
                }

                if (!gameStarted) gameStarted = true;
                yield return new WaitForSeconds(precursorTime);
            }

            Debug.Log("Starting wave " + (i + 1) + " of " + waveValues.Length + " consisting of " + waveValues[i] + " enemies"); ;
            leftInWave = waveValues[i];

            if(i == 0)
            {

                StartCoroutine(MessageManager._instance.DisplayMessages(0, new List<(string, float)>
                { ("They're in the village! Go out there and intercept the enemy!",7)}));
            }

            for (int j = 0; j < waveValues[i]; j++) //Spawn all in order
            {
                int selectedSpawn = Random.Range(0, spawnPoints.Length); //Select random spawn

                GameObject enemy = Instantiate(enemyPrefab,spawnPoints[selectedSpawn].position+Vector3.up,Quaternion.identity); //Spawn enemy. Vector3.up spawns enemy 1 unit above ground.
                enemy.GetComponent<Enemy>().waveManager = this; //Make sure enemy can access this script

                enemies.Add(enemy);

                yield return new WaitForSeconds(spawnDelay);
            }

            Debug.Log("All enemies in wave spawned. Waiting...");
            while (leftInWave > 0) //While still enemies left in wave, wait
            {
                yield return null;
            }

            if (i == 0) //Special condition for Wave 2. Close gate
            {
                GameObject.Find("GatePrefab").GetComponent<GateScript>().CloseGate();
                StartCoroutine(MessageManager._instance.DisplayMessages(0, new List<(string, float)>
        { ("The gate has been closed! Stay out there and defend the gate. The archers will give you cover.",7)}));
            }
            else if (i == 1)
            {
                StartCoroutine(MessageManager._instance.DisplayMessages(0, new List<(string, float)>
                { ("More enemies are coming your way! Prepare yourself!",7)}));
            }

            if((waveValues.Length - i) <= 1) 
            {
                Debug.Log("Last round complete");
                break; //If last round, break immediately without waiting
            }

            else
            {
                wait = Random.Range(endWaveWaitMin, endWaveWaitMax);
                Debug.Log("Next wave starting in " + wait + " seconds");
                yield return new WaitForSeconds(wait);
            }
        }

        yield return new WaitForSeconds(5); //Slight wait before showing victory message

        Debug.Log("Win condition");
        StartCoroutine(MessageManager._instance.DisplayMessages(0, new List<(string, float)>
        { ("The enemy is vanquished! You've successfully held off the attack!",7)}));

        Time.timeScale = 0;
    }

    public void RemoveTarget(GameObject target)
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().potentialTargets.Remove(target);
            }
        }
    }
}
