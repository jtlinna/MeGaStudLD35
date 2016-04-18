using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	enum States {
		playing = 1,
		paused = 2,
		menu = 3};
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if(_instance == null)
                {
                    Debug.LogError("GameManager not found");
                }
            }
            return _instance;
        }
    }

	private static float _lastGameTime;
	private static int _lastScore;

	private States currentState;

    [SerializeField]
    private UIManager UIManager;
	[SerializeField]
    private float gameTime;
	[SerializeField]
    private int score;
	[SerializeField]
    private float maxMultiplier = 10f;
	[SerializeField]
    private float multiplier = 1f;
	[SerializeField]
    private int lives = 5;
    [SerializeField]
    private int bombs = 3;
	[SerializeField]
	private int maxBombs = 5;
    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private Transform PlayerSpawn;
    [SerializeField]
    private float RespawnDelay;

    private BossScript.BossPhase _currentBossPhase;


	void Awake() {

        BossHealth.OnBossDamaged += DebugBossDamaged;
        BossScript.OnBossDied += BossDied;
        Player.OnPlayerDied += PlayerDied;
        Instantiate(PlayerPrefab, PlayerSpawn.position, Quaternion.identity);
        if(UIManager == null)
        {
            UIManager = FindObjectOfType<UIManager>();
            if (UIManager == null)
                Debug.LogError("No UI Manager was found");
        }

        if(UIManager != null)
        {
            UIManager.Init(lives, bombs);
        }

        _currentBossPhase = BossScript.BossPhase.first;
		StartGame ();
	}

    void OnDestroy()
    {
        BossHealth.OnBossDamaged -= DebugBossDamaged;
        BossScript.OnBossDied -= BossDied;
        Player.OnPlayerDied -= PlayerDied;
    }

	public void StartGame() {
		gameTime = 0f;
		currentState = States.playing;
		score = 0;
	}

	public void StopGame() {
		_lastGameTime = gameTime;
		_lastScore = score;

		currentState = States.menu;
        UIManager.ShowEndText(false);

		multiplier = 1f;
		score = 0;
		gameTime = 0f;
	}

	void Update () {
		//if (lives < 0) {
		//	currentState = States.menu;
		//	StopGame();
		//}
		if (currentState == States.playing)
			gameTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.ShowPauseMenu();
        }
	}

    public void DebugBossDamaged(float current, float max)
    {
        Debug.Log("Current: " + current + ", Max: " + max);
    }

    private void BossDied()
    {
        int phase = (int)_currentBossPhase;
        phase++;
        if(phase <= 3)
            _currentBossPhase = (BossScript.BossPhase)phase;
        else
        {
            // TODO Trigger win con
            UIManager.ShowEndText(true);
        }
    }

	public bool addMultiplier (float amount) {
		if (multiplier < maxMultiplier){
			multiplier += amount;
            Debug.Log("Multiplier: " + multiplier);
            UIManager.UpdateScoreMultiplier(multiplier);
			return true;
		} else
			return false; 
	}

	public bool addScore (int amount){
		if (currentState == States.playing) {
			score += (int)((float)amount * multiplier);
            UIManager.UpdateScore(score);
			return true;
		} else {
			Debug.LogError ("GAME NOT RUNNING, NO SCORE ADDED");
			return false;
		}
	}

	public void addLife () {
		lives++;
        UIManager.UpdateLives(lives);
	}

	public void removeLife () {
		lives--;
		multiplier = 1f;
        UIManager.UpdateScoreMultiplier(multiplier);
        UIManager.UpdateLives(lives);
    }

    public int GetBossStage()
    {
        return (int)_currentBossPhase;
    }

    private void PlayerDied()
    {
        removeLife();
        if (lives > 0)
        {
            StartCoroutine(SpawnPlayer(RespawnDelay));
        }
        else
        {
            StopGame();
        }
    }

	public bool addBomb () {
		if (bombs < maxBombs){
			bombs++;
			UIManager.UpdateBombs(bombs);
			return true;
		} else
			return false; 
	}

	public void useBomb () {
		if (FindObjectOfType<BossHealth> () != null)
			FindObjectOfType<BossHealth> ().TakeDamage (20);
		BaseBullet.RemoveAllBullets(true);
		BaseEnemy.RemoveAllEnemies(true);
	}

    IEnumerator SpawnPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);

        Instantiate(PlayerPrefab, PlayerSpawn.position, Quaternion.identity);
    }

}
