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
    public List<TierData> SpawnerTiers;
    public float WaveLength;
}

[System.Serializable]
public class TierData
{
    public List<SpawnerData> Spawners;
    public float StartDelay;
}

[System.Serializable]
public class SpawnerData
{
    public int SpawnerId;
    public List<SpawnCycleData> SpawnCycles;
    public float CycleDelay;
}

[System.Serializable]
public class SpawnCycleData
{
    public int[] Enemies;
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

    public WaveData GetNextWave()
    {
        WaveData waveData = null;
        if(_data.WaveData.Count > 0)
        {
            waveData = _data.WaveData[0];
            _data.WaveData.RemoveAt(0);
        }

        return waveData;
    }

    public TierData GetNextTier(WaveData wave)
    {
        TierData tierData = null;
        if(wave != null)
        {
            if(wave.SpawnerTiers.Count > 0)
            {
                tierData = wave.SpawnerTiers[0];
                wave.SpawnerTiers.RemoveAt(0);
            }
        }

        return tierData;
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
