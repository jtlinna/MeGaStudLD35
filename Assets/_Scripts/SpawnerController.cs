using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour {

    public static System.Action OnBossSpawned;

    [SerializeField]
    private UIManager UIManager;
    [SerializeField]
    private Spawner[] Spawners;

    private WaveData _currentWave;
    private TierData _currentTier;
    private float _timeSinceWaveStart;
    private bool _spawning;

    private List<Spawner> _activeSpawners;

    void Awake()
    {
        StartCoroutine(SetPosition());
        //_currentWave = DataManager.Instance.GetNextWave();
        //if(_currentWave == null)
        //{
            //Debug.LogError("No wave data found -- Disabling SpawnerController");
            //this.enabled = false;
        //}

        //_currentTier = DataManager.Instance.GetNextTier(_currentWave);
        //_timeSinceWaveStart = 0f;
        _activeSpawners = new List<Spawner>();
        _spawning = true;
        StartNewWave();

        for (int i = 0; i < Spawners.Length; i++)
        {
            Spawners[i].OnBossSpawned += BossSpawned;
            Spawners[i].Controller = this;
        }

        BaseEnemy.OnLastEnemyRemoved += StartNewWave;
        BaseEnemy.OnEnemyDied += CheckEnemies;
    }

    void OnDestroy()
    {
        BaseEnemy.OnLastEnemyRemoved -= StartNewWave;
        BaseEnemy.OnEnemyDied -= CheckEnemies;

        for (int i = 0; i < Spawners.Length; i++)
        {
            Spawners[i].OnBossSpawned -= BossSpawned;
        }
    }

    void Update()
    {
        if (!_spawning)
            return;

        _timeSinceWaveStart += Time.deltaTime;
        if(_currentTier != null)
        {
            if (_timeSinceWaveStart >= _currentTier.StartDelay)
            {
                StartTier(_currentTier);
                _currentTier = DataManager.Instance.GetNextTier(_currentWave);
                while(_currentTier != null && _timeSinceWaveStart >= _currentTier.StartDelay)
                {
                    StartTier(_currentTier);
                    _currentTier = DataManager.Instance.GetNextTier(_currentWave);
                }
            }
        }

        if(_currentWave != null &&_timeSinceWaveStart >= _currentWave.WaveLength)
        {
            StartNewWave();
        }
    }

    public void StartNewWave()
    {
        if (_activeSpawners.Count > 0 || !_spawning)
            return;

        if(DataManager.Instance == null)
            return;

        _timeSinceWaveStart = 0f;
        
        _currentWave = DataManager.Instance.GetNextWave();
        if (_currentWave == null)
        {
            Debug.Log("No more waves");
            this.enabled = false;
            return;
        }
        Debug.Log("Starting wave " + _currentWave.WaveId);
        _currentTier = DataManager.Instance.GetNextTier(_currentWave);
    }

    private void StartTier(TierData tier)
    {
        for (int i = 0; i < tier.Spawners.Count; i++)
        {
            for (int j = 0; j < Spawners.Length; j++)
            {
                if (Spawners[j].SpawnerId == tier.Spawners[i].SpawnerId)
                {
                    Spawners[j].StartSpawnCycles(tier.Spawners[i].SpawnCycles, tier.Spawners[i].CycleDelay, RemoveSpawner);

                    if (_activeSpawners == null)
                        _activeSpawners = new List<Spawner>();
                    _activeSpawners.Add(Spawners[j]);
                    break;
                }
            }
        }
    }
    
    public bool CanSpawnBoss(Spawner spawner)
    {
        if(_activeSpawners.Count == 0 && BaseEnemy.GetActiveEnemies() == 0)
        {
            return true;
        }

        return false;
    }

    private void RemoveSpawner(Spawner spawner)
    {
        if(_activeSpawners.Contains(spawner))
        {
            _activeSpawners.Remove(spawner);
        }
    }

    private void BossSpawned()
    {
        _spawning = false;
        BossScript.OnBossDied += BossDied;
        if(OnBossSpawned != null)
        {
            OnBossSpawned();
        }
    }

    public void DisableSpawning()
    {
        _spawning = false;
    }

    private void BossDied()
    {
        BossScript.OnBossDied -= BossDied;
        _spawning = true;
        _activeSpawners.Clear();
        StartNewWave();
    }

    private void CheckEnemies()
    {
        if(BaseEnemy.GetActiveEnemies() == 0)
        {
            StartNewWave();
        }
    }

    private IEnumerator SetPosition()
    {
        yield return null;

        Vector3 pos = transform.position;
        pos.y = Mathf.Abs(UIManager.GetCorner(UICorners.TOP_LEFT).y) + 2.5f;
        transform.position = pos;
    }
    // Second iteration

    //void Update()
    //{
    //    if (_spawning)
    //        return;

    //    _timerSinceLastSpawn += Time.deltaTime;
    //    if (_timerSinceLastSpawn >= _waveData[_currentWave].StartDelay)
    //    {
    //        _spawning = true;
    //        StartCoroutine(SpawnRoutine());
    //    }
    //}

    //private IEnumerator SpawnRoutine()
    //{
    //    WaitForSeconds spawnerWait = new WaitForSeconds(_waveData[_currentWave].SpawnDelay);

    //    for (int i = 0; i < _waveData[_currentWave].SpawnerTiers.Length; i++)
    //    {
    //        SpawnerTierData tier = _waveData[_currentWave].SpawnerTiers[i];
    //        WaitForSeconds interval = new WaitForSeconds(tier.Interval);

    //        int spawnedCount = 0;
    //        while (spawnedCount < tier.SpawnCount)
    //        {
    //            for (int j = 0; j < tier.SpawnerId.Length; j++)
    //            {
    //                Spawn(tier.SpawnerId[j], tier.EnemyId[j]);
    //                yield return null;
    //            }
    //            spawnedCount++;
    //            yield return interval;
    //        }

    //        yield return spawnerWait;
    //    }

    //    _spawning = false;
    //}

    //private void Spawn(int spawnerId, int enemyId)
    //{
    //    if(_prefab == null)
    //    {
    //        GameObject go = Resources.Load<GameObject>("Enemies/EnemyPrefab");

    //        if(go == null)
    //        {
    //            Debug.LogError("No enemy prefab found");
    //            return;
    //        }

    //        _prefab = go.GetComponent<BaseEnemy>();

    //        if(_prefab == null)
    //        {
    //            Debug.LogError("No BaseEnemy script was found in the enemy prefab");
    //            return;
    //        }
    //    }

    //    BaseEnemy enemy = Instantiate(_prefab, SpawnPoints[spawnerId].position, Quaternion.identity) as BaseEnemy;
    //    if (enemy != null)
    //    {
    //        enemy.Type = (EnemyIdentifier)enemyId;
    //        Debug.Log("Enemy type: " + enemy.Type.ToString());
    //        enemy.ChangeShape(false, false);
    //    }
    //}

    // First iteration

    //[SerializeField]
    //private EnemyIdentifier EnemyType;
    //[SerializeField]
    //private float SpawnInterval;

    //private float _timer;
    //private GameObject _prefab;
    
    //void Update()
    //{
    //    _timer -= Time.deltaTime;
    //    if(_timer <= 0f)
    //    {
    //        _timer = SpawnInterval;
    //        Spawn();
    //    }
    //}

    //void Spawn()
    //{
    //    if(_prefab == null)
    //    {
    //        _prefab = GetPrefab();

    //        if(_prefab == null)
    //        {
    //            Debug.LogError("No enemy prefab found!");
    //            return;
    //        }
    //    }

    //    GameObject go = Instantiate(_prefab, transform.position, Quaternion.identity) as GameObject;
    //    BaseEnemy enemy = go.GetComponent<BaseEnemy>();
    //    if(enemy != null)
    //    {
    //        enemy.Type = EnemyType;
    //        enemy.Init();
    //    }
    //}

    //GameObject GetPrefab()
    //{
    //    return Resources.Load<GameObject>("Enemies/EnemyPrefab");
        
    //}
}
