using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public enum UICorners
{
    TOP_LEFT = 0,
    TOP_RIGHT = 1,
    BOTTOM_LEFT = 2,
    BOTTOM_RIGHT = 3
}

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
    private Transform[] Corners;

    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]
    private Button ToggleSoundsButton;
    [SerializeField]
    private Text ToggleSoundsText;

    [SerializeField]
    private GameObject EndButtonsContainer;
    [SerializeField]
    private Button MainMenuButton;
    [SerializeField]
    private Button RestartButton;

    [SerializeField]
    private GameObject HighscoreContainer;
    [SerializeField]
    private InputField NameInput;
    [SerializeField]
    private Button SubmitButton;
    
    void Awake()
    {
        SpawnerController.OnBossSpawned += ShowBossHealthbar;
        BossScript.OnBossDied += HideBossHealthbar;
        BossHealth.OnBossDamaged += UpdateBossHealth;
        HidePauseMenu();
        HighscoreContainer.SetActive(false);
        EndButtonsContainer.SetActive(false);

        MainMenuButton.onClick.RemoveAllListeners();
        MainMenuButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("_MainMenu");
        });

        RestartButton.onClick.RemoveAllListeners();
        RestartButton.onClick.AddListener(delegate {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        ToggleSoundsButton.onClick.RemoveAllListeners();
        ToggleSoundsButton.onClick.AddListener(delegate
        {
            ToggleSounds();
        });
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
        bool soundsOn = SoundManager.Instance.SoundsOn();
        ToggleSoundsText.text = (soundsOn) ? "SOUNDS ON" : "SOUNDS OFF";
        Time.timeScale = 0f;
    }

    public void HidePauseMenu()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString("D10");
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

    public void ShowHighscoreDialog(System.Action<string> callback)
    {
        HighscoreContainer.SetActive(true);
        SubmitButton.onClick.RemoveAllListeners();
        SubmitButton.onClick.AddListener(delegate {
            if(!string.IsNullOrEmpty(NameInput.text))
            {
                callback(NameInput.text);
                HighscoreContainer.SetActive(false);
                ShowEndButtons();
            }
        });
    }

    public void ShowEndButtons()
    {
        EndButtonsContainer.SetActive(true);
    }

    public Vector3 GetCorner(UICorners corner)
    {
        return Camera.main.ScreenToWorldPoint(Corners[(int)corner].position);
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

    private void ToggleSounds()
    {
        bool soundsOn = SoundManager.Instance.SoundsOn();
        SoundManager.Instance.ToggleSounds(!soundsOn);
        ToggleSoundsText.text = (soundsOn) ? "SOUNDS OFF" : "SOUNDS ON";
    }
}
