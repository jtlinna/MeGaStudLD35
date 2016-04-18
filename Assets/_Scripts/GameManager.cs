using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private GameObject _currentPlayer;

    private BossScript.BossPhase _currentBossPhase;


	void Awake() {
        
        BossScript.OnBossDied += BossDied;
        Player.OnPlayerDied += PlayerDied;
        _currentPlayer = Instantiate(PlayerPrefab, PlayerSpawn.position, Quaternion.identity) as GameObject;
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

        if(LeaderboardManager.Instance.IsHighscore(score))
        {
            UIManager.ShowHighscoreDialog(SubmitHighscore);
        }
        else
        {
            UIManager.ShowEndButtons();
            score = 0;
        }


		multiplier = 1f;
		gameTime = 0f;
	}

    private void SubmitHighscore(string name)
    {
        LeaderboardManager.Instance.AddScore(name, score);
        score = 0;
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
            UIManager.UpdateScoreMultiplier(multiplier);
			return true;
		} else
			return false; 
	}

	public bool addScore (int amount){
		if (currentState == States.playing) {
            amount *= 10;
			score += (int)((float)amount * multiplier);
            UIManager.UpdateScore(score);
			return true;
		} else {
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

	public bool useBomb () {
		if (bombs > 0) {
			bombs--;
			if (FindObjectOfType<BossHealth> () != null)
				FindObjectOfType<BossHealth> ().TakeDamage (20);
			BaseBullet.RemoveAllBullets (true);
			BaseEnemy.RemoveAllEnemies (true);
			UIManager.UpdateBombs (bombs);
			StartCoroutine (PowerUpsToPlayer (PowerUp.GetActivePowerUps ()));
			StartCoroutine (bombFlash ());
			SoundManager.Instance.PlayClip ((SoundType)Random.Range(1,3));
			return true;
		} else
			return false;
	}

	private IEnumerator bombFlash () {
		yield return null;
		SpriteRenderer flash = GameObject.Find ("BombFlash").GetComponent<SpriteRenderer> ();
		flash.color = new Color (1f,1f,1f,1f);
		while (flash.color.a > 0f){
			flash.color = new Color (1f, 1f, 1f, flash.color.a - 0.025f);
			yield return new WaitForEndOfFrame ();
		}
	}

    IEnumerator PowerUpsToPlayer(List<PowerUp> powerups)
    {
        while(powerups.Count > 0 && _currentPlayer != null)
        {
            for(int i = powerups.Count - 1; i >= 0; i--)
            {
                powerups[i].transform.position = Vector3.MoveTowards(powerups[i].transform.position, _currentPlayer.transform.position, 20f * Time.deltaTime);
                if(powerups[i].transform.position == _currentPlayer.transform.position)
                {
                    powerups.RemoveAt(i);
                }
            }

            yield return null;
        }
    }

    IEnumerator SpawnPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);

        _currentPlayer = Instantiate(PlayerPrefab, PlayerSpawn.position, Quaternion.identity) as GameObject;
    }

}
