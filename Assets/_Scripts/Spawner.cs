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
        _timer = SpawnInterval;
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
            string prefabPath = GetPrefabPath();
            _prefab = Resources.Load<GameObject>(prefabPath);
            if(_prefab == null)
            {
                Debug.LogError("No prefab for type " + EnemyType.ToString() + " was found");
                return;
            }
        }

        Instantiate(_prefab, transform.position, Quaternion.identity);
    }

    string GetPrefabPath()
    {
        string path = "Enemies/";

        switch(EnemyType)
        {
            case EnemyIdentifier.DIAMOND_ENEMY:
                path += "DiamondEnemyPrefab";
                break;
        }

        return path;
    }
}
