using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Transform LivesContainer;
    [SerializeField]
    private GameObject LifeIconPrefab;

    [SerializeField]
    private Transform BombsContainer;
    [SerializeField]
    private GameObject BombIconPrefab;

    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text ScoreMultiplierText;

    public void Init(int maxLives, int maxBombs)
    {
        if(LivesContainer.childCount > 0)
        {
            for(int i = LivesContainer.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(LivesContainer.GetChild(i).gameObject);
            }
        }

        if(BombsContainer.childCount > 0)
        {
            for(int i = BombsContainer.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(BombsContainer.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < maxLives; i++)
        {
            GameObject go = Instantiate(LifeIconPrefab) as GameObject;
            go.transform.SetParent(LivesContainer, false);
        }

        for (int i = 0; i < maxBombs; i++)
        {
            GameObject go = Instantiate(BombIconPrefab) as GameObject;
            go.transform.SetParent(BombsContainer, false);
        }
        UpdateScore(0);
        UpdateScoreMultiplier(1);
    }

    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString("D8");
    }

    public void UpdateScoreMultiplier(int multiplier)
    {
        ScoreMultiplierText.text = "X" + multiplier;
    }
}
