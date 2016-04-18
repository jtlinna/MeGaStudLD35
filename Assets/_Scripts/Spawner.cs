using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    public System.Action OnBossSpawned;

    private System.Action<Spawner> OnSpawnCyclesComplete;

    public int SpawnerId;
    public SpawnerController Controller;

    private List<SpawnCycleData> _currentCycles;

    private GameObject _prefab;
    private GameObject _bossPrefab;
    private float _cycleDelay;


    void Awake()
    {
        _currentCycles = new List<SpawnCycleData>();
    }

    public void StartSpawnCycles(List<SpawnCycleData> cycles, float cycleDelay, System.Action<Spawner> callback)
    {
        Debug.Log("Start cycle");
        _currentCycles = cycles;
        _cycleDelay = cycleDelay;
        OnSpawnCyclesComplete = callback;

        StopCoroutine("SpawnRoutine");
        StartCoroutine("SpawnRoutine");
    }

    void Spawn(int enemyId, int powerUpId)
    {
        if((EnemyIdentifier)enemyId == EnemyIdentifier.BOSS)
        {
            StartCoroutine(SpawnBossRoutine());
            return;
        }

        int index = enemyId - 3;
        if (_prefab == null)
        {
            _prefab = GetPrefab();

            if (_prefab == null)
            {
                Debug.LogError("No enemy prefab found!");
                return;
            }
        }

        GameObject go = Instantiate(_prefab, transform.position, Quaternion.identity) as GameObject;
        BaseEnemy enemy = go.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.Type = (EnemyIdentifier)enemyId;
            enemy.Init((PowerUpIdentifier)powerUpId);
        }
    }

    private void SpawnBoss()
    {
        if(_bossPrefab == null)
        {
            _bossPrefab = Resources.Load<GameObject>("Enemies/Boss");
            if(_bossPrefab == null)
            {
                Debug.LogError("No boss prefab was found");
                return;
            }
        }

        GameObject go = Instantiate(_bossPrefab, transform.position, Quaternion.identity) as GameObject;
        BossScript boss = go.GetComponent<BossScript>();
        if(boss != null)
        {
            boss.phase = (BossScript.BossPhase)GameManager.Instance.GetBossStage();
            boss.Init();
        }

        if(OnBossSpawned != null)
        {
            OnBossSpawned();
        }
    }

    GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("Enemies/EnemyPrefab");
    }

    private IEnumerator SpawnRoutine()
    {
        while (_currentCycles.Count > 0)
        {
            int index = 0;
            while(index < _currentCycles[0].Enemies.Length)
            {
                Spawn(_currentCycles[0].Enemies[index], _currentCycles[0].PowerUps[index]);
                index++;
                yield return new WaitForSeconds(_currentCycles[0].Interval);
            }
            _currentCycles.RemoveAt(0);
            if(_currentCycles.Count > 0)
                yield return new WaitForSeconds(_cycleDelay);
        }

        if(OnSpawnCyclesComplete != null)
        {
            OnSpawnCyclesComplete(this);
        }
    }

    IEnumerator SpawnBossRoutine()
    {
        Controller.DisableSpawning();

        while(!Controller.CanSpawnBoss(this))
        {
            yield return null;
        }

        SpawnBoss();
    }

    IEnumerator CheckRemainingEnemies()
    {
        yield return null;
    }
}
