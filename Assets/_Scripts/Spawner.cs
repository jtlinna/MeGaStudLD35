using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    private System.Action<Spawner> OnSpawnCyclesComplete;

    public int SpawnerId;

    private List<SpawnCycleData> _currentCycles;

    private GameObject[] _prefabs;
    private float _cycleDelay;


    void Awake()
    {
        _prefabs = new GameObject[System.Enum.GetNames(typeof(EnemyIdentifier)).Length];
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
        int index = enemyId - 3;
        if (_prefabs[index] == null)
        {
            _prefabs[index] = GetPrefab();

            if (_prefabs[(int)index] == null)
            {
                Debug.LogError("No enemy prefab found!");
                return;
            }
        }

        GameObject go = Instantiate(_prefabs[(int)index], transform.position, Quaternion.identity) as GameObject;
        BaseEnemy enemy = go.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.Type = (EnemyIdentifier)enemyId;
            enemy.Init((PowerUpIdentifier)powerUpId);
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
}
