using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ConfigurationData
{
    public List<WaveData> WaveData;
}

[System.Serializable]
public class WaveData
{
    public int WaveId;
    public SpawnerTierData[] SpawnerTiers;
    public float StartDelay;
    public float SpawnDelay;
    public float WaveDelay;
}

[System.Serializable]
public class SpawnerTierData
{
    public int[] SpawnerId;
    public int[] EnemyId;
    public int SpawnCount;
    public float Interval;
}

[System.Serializable]
public class ConfigDataWrapper
{
    public List<WaveData> WaveConfigData;
}

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    public static DataManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DataManager>();

                if(_instance == null)
                {
                    Debug.LogError("No DataManager was found");
                }
                else
                {
                    _instance.FetchConfiguration();
                }
            }

            return _instance;
        }
    }

    private ConfigurationData _data;

    public List<WaveData> GetWaveData()
    {
        return _data.WaveData;
    }

    private bool FetchConfiguration()
    {
        _data = new ConfigurationData();
        TextAsset asset = Resources.Load<TextAsset>("Data/wave_data");
        
        if(asset != null)
        {
            string configString = asset.text;
            ConfigDataWrapper deserializedData = JsonUtility.FromJson<ConfigDataWrapper>(configString);
            _data.WaveData = deserializedData.WaveConfigData;

            string debugSerialize = JsonUtility.ToJson(deserializedData);
            Debug.Log("Serialized: " + debugSerialize);
        }

        return true;
    }
}
