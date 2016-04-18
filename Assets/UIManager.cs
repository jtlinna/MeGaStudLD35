using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text LivesText;

    [SerializeField]
    private Text BombsText;

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
       
        //for (int i = 0; i < maxLives; i++)
        //{
        //    GameObject go = Instantiate(LifeIconPrefab) as GameObject;
        //    go.transform.SetParent(LivesContainer, false);
        //}

        UpdateLives(lives);
        UpdateBombs(bombs);
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
        LivesText.text = "X" + lives;
    }

    public void UpdateBombs(int bombs)
    {
        BombsText.text = "X" + bombs;
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
