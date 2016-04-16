using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour {

    [SerializeField]
    private Transform[] SpawnPoints;

    private int _currentWave;
    private float _timerSinceLastSpawn;

    private List<BaseAI> _spawnedEnemies;
    private List<WaveData> _waveData;

    private BaseEnemy _prefab;

    private bool _spawning;

    void Awake()
    {
        _spawning = false;
        _currentWave = 0;
        _waveData = DataManager.Instance.GetWaveData();
        if(_waveData == null)
        {
            Debug.LogError("No wave data found -- Disabling SpawnerController");
        }        
    }

    void Update()
    {
        if (_spawning)
            return;

        _timerSinceLastSpawn += Time.deltaTime;
        if(_timerSinceLastSpawn >= _waveData[_currentWave].StartDelay)
        {
            _spawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds spawnerWait = new WaitForSeconds(_waveData[_currentWave].SpawnDelay);

        for (int i = 0; i < _waveData[_currentWave].SpawnerTiers.Length; i++)
        {
            SpawnerTierData tier = _waveData[_currentWave].SpawnerTiers[i];
            WaitForSeconds interval = new WaitForSeconds(tier.Interval);

            int spawnedCount = 0;
            while (spawnedCount < tier.SpawnCount)
            {
                for (int j = 0; j < tier.SpawnerId.Length; j++)
                {
                    Debug.Log("SPAWN");
                    Spawn(tier.SpawnerId[j]);
                    yield return null;
                }
                spawnedCount++;
                yield return interval;
            }

            yield return spawnerWait;
        }

        _spawning = false;
    }

    private void Spawn(int spawnerId)
    {
        if(_prefab == null)
        {
            GameObject go = Resources.Load<GameObject>("Enemies/EnemyPrefab");

            if(go == null)
            {
                Debug.LogError("No enemy prefab found");
                return;
            }

            _prefab = go.GetComponent<BaseEnemy>();

            if(_prefab == null)
            {
                Debug.LogError("No BaseEnemy script was found in the enemy prefab");
                return;
            }
        }

        BaseEnemy enemy = Instantiate(_prefab, SpawnPoints[spawnerId].position, Quaternion.identity) as BaseEnemy;
        if (enemy != null)
        {
            enemy.Type = (EnemyIdentifier)(spawnerId + 1);
            enemy.ChangeShape();
        }
    }


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
