using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaderboardContainer : MonoBehaviour {

    [SerializeField]
    private Text Name;
    [SerializeField]
    private Text Score;

	public void Init(LeaderboardEntry entry)
    {
        Name.text = entry.Name;
        Score.text = entry.Score.ToString();
    }
}
