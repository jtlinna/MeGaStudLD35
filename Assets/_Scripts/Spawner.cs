using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private EnemyIdentifier EnemyType;
    [SerializeField]
    private float SpawnInterval;

    private float _timer;
    private GameObject _prefab;

    void Awake()
    {
        //_timer = SpawnInterval;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if(_timer <= 0f)
        {
            _timer = SpawnInterval;
            Spawn();
        }
    }

    void Spawn()
    {
        if(_prefab == null)
        {
            _prefab = GetPrefab();

            if(_prefab == null)
            {
                Debug.LogError("No enemy prefab found!");
                return;
            }
        }

        GameObject go = Instantiate(_prefab, transform.position, Quaternion.identity) as GameObject;
        BaseEnemy enemy = go.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            enemy.Type = EnemyType;
            enemy.Init();
        }
    }

    GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("Enemies/EnemyPrefab");
        
    }
}
