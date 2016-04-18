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

    [SerializeField]
    private Text EndText;

    [SerializeField]
    private GameObject BossHealthbarParent;
    [SerializeField]
    private Image BossHealthbar;

    [SerializeField]
    private GameObject PauseMenu;

    void Awake()
    {
        SpawnerController.OnBossSpawned += ShowBossHealthbar;
        BossScript.OnBossDied += HideBossHealthbar;
        BossHealth.OnBossDamaged += UpdateBossHealth;
        HidePauseMenu();
    }

    void OnDestroy()
    {
        SpawnerController.OnBossSpawned -= ShowBossHealthbar;
        BossScript.OnBossDied -= HideBossHealthbar;
        BossHealth.OnBossDamaged -= UpdateBossHealth;
    }

    public void Init(int lives, int bombs)
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
        //for (int i = 0; i < maxLives; i++)
        //{
        //    GameObject go = Instantiate(LifeIconPrefab) as GameObject;
        //    go.transform.SetParent(LivesContainer, false);
        //}

        UpdateLives(lives);

        for (int i = 0; i < bombs; i++)
        {
            GameObject go = Instantiate(BombIconPrefab) as GameObject;
            go.transform.SetParent(BombsContainer, false);
        }
        UpdateScore(0);
        UpdateScoreMultiplier(1);
        EndText.gameObject.SetActive(false);
        HideBossHealthbar();
    }

    public void ShowPauseMenu()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HidePauseMenu()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString("D8");
    }

    public void UpdateScoreMultiplier(float multiplier)
    {
        ScoreMultiplierText.text = "X" + multiplier;
    }

    public void UpdateLives(int lives)
    {
        if(lives > LivesContainer.childCount)
        {
            while (LivesContainer.childCount < lives)
            {
                GameObject go = Instantiate(LifeIconPrefab) as GameObject;
                go.transform.SetParent(LivesContainer, false);
            }
        }
        else
        {
            for (int i = 0; i < LivesContainer.childCount; i++)
            {
                LivesContainer.GetChild(i).gameObject.SetActive(i < lives);
            }
        }
    }

    public void ShowEndText(bool win)
    {
        string endText = win ? "YOU WIN!" : "YOU LOSE";
        EndText.text = endText;
        EndText.gameObject.SetActive(true);
    }

    public void UpdateBossHealth(float current, float max)
    {
        float percentage = current / max;
        BossHealthbar.fillAmount = Mathf.Clamp01(percentage);
    }

    private void ShowBossHealthbar()
    {
        BossHealthbar.fillAmount = 1f;
        BossHealthbarParent.gameObject.SetActive(true);
    }

    private void HideBossHealthbar()
    {
        BossHealthbarParent.gameObject.SetActive(false);
    }
}
