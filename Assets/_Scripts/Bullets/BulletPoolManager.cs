using UnityEngine;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour
{
    private static BulletPoolManager _instance;
    public static BulletPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BulletPoolManager>();
                if (_instance == null)
                {
                    Debug.LogError("No BulletPoolManager was found");
                }
            }

            return _instance;
        }
    }

    [SerializeField]
    private int InitialPool = 10;

    private Dictionary<BulletType, List<GameObject>> _bulletMap;
    private Dictionary<BulletType, GameObject> _prefabMap;
    private Transform _bulletParent;

    void Awake()
    {
        _bulletParent = new GameObject("Bullets").transform;
        _bulletParent.transform.position = Vector3.zero;
        _bulletParent.rotation = Quaternion.identity;

        _bulletMap = new Dictionary<BulletType, List<GameObject>>();
        _prefabMap = new Dictionary<BulletType, GameObject>();

        string[] bulletNames = System.Enum.GetNames(typeof(BulletType));
        for (int i = 0; i < bulletNames.Length; i++)
        {
            BulletType type = (BulletType)System.Enum.Parse(typeof(BulletType), bulletNames[i]);
            _prefabMap.Add(type, FetchPrefab(type));
            List<GameObject> bullets = new List<GameObject>();
            for (int j = 0; j < InitialPool; j++)
            {
                GameObject go = Instantiate(_prefabMap[type]);
                go.SetActive(false);
                bullets.Add(go);
            }
            _bulletMap.Add(type, bullets);
        }
    }

    public GameObject Instantiate(BulletType type, Vector3 position, Quaternion rotation)
    {
        GameObject go = null;
        if (_bulletMap.ContainsKey(type))
        {
            List<GameObject> bullets = _bulletMap[type];
            bool found = false;
            for (int i = 0; i < bullets.Count; i++)
            {
                if(!bullets[i].activeSelf)
                {
                    go = bullets[i];
                    go.transform.position = position;
                    go.transform.rotation = rotation;
                    go.SetActive(true);
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                go = GameObject.Instantiate(_prefabMap[type], position, rotation) as GameObject;
                bullets.Add(go);
            }
        }
        else
        {
            List<GameObject> bullets = new List<GameObject>();
            if (!_prefabMap.ContainsKey(type))
            {
                _prefabMap.Add(type, FetchPrefab(type));
            }

            if (_prefabMap.ContainsKey(type))
            {
                go = GameObject.Instantiate(_prefabMap[type], position, rotation) as GameObject;
                bullets.Add(go);
                _bulletMap.Add(type, bullets);
            }
            else
            {
                Debug.LogError("No bullet prefab found for bullet " + type.ToString());
            }
        }
        return go;
    }

    public GameObject Instantiate(BulletType type)
    {
        return this.Instantiate(type, Vector3.zero, Quaternion.identity);
    }

    private GameObject FetchPrefab(BulletType type)
    {
        string path = "Bullets/";
        switch (type)
        {
            case BulletType.PLAYER:
                path += "PlayerBasicBullet";
                break;
            case BulletType.TRIANGLE:
                path += "triangleBullet";
                break;
            case BulletType.SQUARE:
                path += "squareBullet";
                break;
            case BulletType.PENTAGON:
                path += "pentaBullet";
                break;
            case BulletType.HEXAGON:
                path += "hexaBullet";
                break;
            case BulletType.SEPTIGON:
                path += "septaBullet";
                break;
            case BulletType.OCTAGON:
                path += "octaBullet";
                break;
            case BulletType.BOSS_S1:
                path += "BossS1Bullet";
                break;
            case BulletType.BOSS_S3:
                path += "BossS3Bullet";
                break;
            case BulletType.BOSS_S4:
                path += "BossS4Bullet";
                break;
            case BulletType.BOSS_S5:
                path += "BossS5Bullet";
                break;
        }

        GameObject go = Resources.Load<GameObject>(path);
        if(go == null)
        {
            Debug.LogError("No prefab found for bullet " + type.ToString());
        }

        return go;
    }
}
