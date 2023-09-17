using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject hope;
    bool wavesEnded = false;
    public CountdownTimer script;
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform[] enemy;
        public int[] count;
        public float[] rate;
    }

    public Wave[] waves;
    [SerializeField] private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave + 1; }
    }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;

    [SerializeField] private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }

    public UnityEvent OnWaveComplete;
    public UnityEvent OnWaveStart;
    private Coroutine Spawning;
    Coroutine[] Spawners;

    void Start()
    {
        Spawners = new Coroutine[7];
        script = GameObject.FindWithTag("Canvas").GetComponent<CountdownTimer>();
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced.");

        }
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (pause_menu.gameIsPaused || wavesEnded) return;
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                script.currentTime = timeBetweenWaves;
                WaveCompleted();
            }
            else
            {
                return;
            }
        }
        // Starts a wave
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                Debug.Log("Trigger Wave");
                Spawning = StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        OnWaveComplete.Invoke();

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            Instantiate(hope, new Vector2(0, -5), hope.transform.rotation);
            wavesEnded=true;
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null && GameObject.FindGameObjectWithTag("Boss") == null)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetWave()
    {
        try { StopCoroutine(Spawning); } catch { }
        for (int i = 0; i < Spawners.Length; i++)
        {
            try
            {
                StopCoroutine(Spawners[i]);
            }
            catch { }
        }
        KillAllEnemies();
        state = SpawnState.COUNTING;
        OnWaveComplete?.Invoke();
        waveCountdown = timeBetweenWaves;
        script.currentTime = waveCountdown;
    }

    private void KillAllEnemies()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < bosses.Length; i++)
        {
            Destroy(bosses[i]);
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }

    private void DestroyAllItemsOnGround()
    {
        GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");
        GameObject[] swords = GameObject.FindGameObjectsWithTag("Sword");
        for (int i = 0; i < guns.Length; i++)
        {
            if (!guns[i].GetComponent<gun_controller>().held)
            {
                Destroy(guns[i]);
            }

        }
        for (int i = 0; i < swords.Length; i++)
        {
            if (!swords[i].GetComponent<sword_controller>().held)
            {
                Destroy(swords[i]);
            }

        }
    }

    bool[] allFinished;
    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;
        Invoke("DestroyAllItemsOnGround", 0.15f); 
        yield return null;

        OnWaveStart?.Invoke();
        allFinished = new bool[_wave.enemy.Length];

        // Spawns each enemy
        for (int i = 0; i < _wave.enemy.Length; i++)
        {
            Debug.Log(i.ToString() + ": Spawn this type");
            allFinished[i] = false;
            Spawners[i] = StartCoroutine(SpawnWaveEnemeies(_wave, i));
        }

        // Check if all done spawning
        bool finished = false;
        while (!finished)
        {
            yield return null;
            finished = AllTrue(allFinished);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    IEnumerator SpawnWaveEnemeies(Wave _wave, int index)
    {
        for (int i = 0; i < _wave.count[index]; i++)
        {
            SpawnEnemy(_wave.enemy[index]);
            yield return new WaitForSeconds(_wave.rate[index]);
        }
        allFinished[index] = true;
    }

    private bool AllTrue(bool[] list)
    {
        bool returnTrue = true;
        for (int i = 0; i < list.Length; i++)
        {
            returnTrue = returnTrue && list[i];
        }
        return returnTrue;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    private void OnEnable()
    {
        pause_menu.OnRetry += ResetWave;
    }

    private void OnDisable()
    {
        pause_menu.OnRetry -= ResetWave;
    }


}
