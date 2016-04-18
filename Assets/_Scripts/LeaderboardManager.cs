using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LeaderboardEntry
{
    public string Name;
    public int Score;
}

[SerializeField]
public class Leaderboard
{
    public List<LeaderboardEntry> Entries;
}

public class LeaderboardManager : MonoBehaviour {

    private static LeaderboardManager _instance;
    public static LeaderboardManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<LeaderboardManager>();
                if(_instance == null)
                {
                    Debug.LogError("LeaderboarManager not found");
                }
            }

            return _instance;
        }
    }

    private Leaderboard _leaderboard;
    
    public void AddScore(string name, int score)
    {
        if(_leaderboard == null)
        {
            _leaderboard = GetLeaderboard();
        }

        LeaderboardEntry entry = new LeaderboardEntry();
        entry.Name = name;
        entry.Score = score;

        for (int i = 0; i < _leaderboard.Entries.Count; i++)
        {
            if (score > _leaderboard.Entries[i].Score)
            {
                _leaderboard.Entries.Insert(i, entry);
                _leaderboard.Entries.RemoveAt(_leaderboard.Entries.Count - 1);
                break;
            }
        }

        string leaderboardString = JsonUtility.ToJson(_leaderboard);
        PlayerPrefs.SetString("Leaderboard", leaderboardString);
    }

    public bool IsHighscore(int score)
    {
        if(_leaderboard == null)
        {
            _leaderboard = GetLeaderboard();
        }

        if(_leaderboard != null)
        {
            for (int i = 0; i < _leaderboard.Entries.Count; i++)
            {
                if (score > _leaderboard.Entries[i].Score)
                    return true;
            }
        }

        return false;
    }

    public Leaderboard GetLeaderboard()
    {
        Leaderboard leaderboard = null;

        if (PlayerPrefs.HasKey("Leaderboard"))
        {
            leaderboard = JsonUtility.FromJson<Leaderboard>(PlayerPrefs.GetString("Leaderboard"));
        }
        else
        {
            leaderboard = GenerateLeaderboard();
        }

        return leaderboard;
    }

    private Leaderboard GenerateLeaderboard()
    {
        Leaderboard leaderboard = new Leaderboard();
        leaderboard.Entries = new List<LeaderboardEntry>();
        for (int i = 0; i < 10; i++)
        {
            LeaderboardEntry data = new LeaderboardEntry();
            data.Name = "Empty";
            data.Score = 0;
            leaderboard.Entries.Add(data);
        }

        string leaderboardString = JsonUtility.ToJson(leaderboard);
        PlayerPrefs.SetString("Leaderboard", leaderboardString);

        return leaderboard;
    }
    
}
