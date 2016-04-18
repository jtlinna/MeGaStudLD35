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
        
        BaseEnemy.OnEnemyDied += CheckEnemies;
    }

    void OnDestroy()
    {
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
			#if UNITY_EDITOR
            Debug.Log("No more waves");
			#endif
            this.enabled = false;
            return;
        }
		#if UNITY_EDITOR
        Debug.Log("Starting wave " + _currentWave.WaveId);
		#endif
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
			#if UNITY_EDITOR
            Debug.Log("Last enemy killed, active spawner: " + _activeSpawners.Count);
			#endif
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
}
