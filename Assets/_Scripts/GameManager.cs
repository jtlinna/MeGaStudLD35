using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	enum States {
		playing = 1,
		paused = 2,
		menu = 3};

    public static int BossStage = 1;

	private States currentState;
	[SerializeField] private float gameTime;
	[SerializeField] private ulong score;
	[SerializeField] private float maxMultiplier = 10f;
	[SerializeField] private float multiplier = 1f;
	[SerializeField] private int lives = 5;
    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private Transform PlayerSpawn;
    [SerializeField]
    private float RespawnDelay;

	private float _lastGameTime;
	public float lastGameTime { get { return _lastGameTime; } }
	private ulong _lastScore;
	public ulong lastScore { get { return _lastScore; } }

	void Awake() {
		DontDestroyOnLoad(gameObject);

        BossHealth.OnBossDamaged += DebugBossDamaged;
        Player.OnPlayerDied += PlayerDied;
        Instantiate(PlayerPrefab, PlayerSpawn.position, Quaternion.identity);
		StartGame ();
	}

    void OnDestroy()
    {
        BossHealth.OnBossDamaged -= DebugBossDamaged;
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

		multiplier = 1f;
		score = 0;
		gameTime = 0f;
	}

	void Update () {
		if (lives < 0) {
			currentState = States.menu;
			StopGame();
		}
		if (currentState == States.playing)
			gameTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.K))
        {
            BaseBullet.RemoveAllBullets(true);
            BaseEnemy.RemoveAllEnemies(true);
        }
	}

    public void DebugBossDamaged(float current, float max)
    {
        Debug.Log("Current: " + current + ", Max: " + max);
    }

	public bool addMultiplier (float amount) {
		if (multiplier < maxMultiplier){
			multiplier += amount;
			return true;
		} else
			return false; 
	}

	public bool addScore (ulong amount){
		if (currentState == States.playing) {
			score += (ulong)((float)amount * multiplier);
			return true;
		} else {
			Debug.LogError ("GAME NOT RUNNING, NO SCORE ADDED");
			return false;
		}
	}

	public void addLife () {
		lives++;
	}

	public void removeLife () {
		lives--;
		multiplier = 1f;
	}

    private void PlayerDied()
    {
        removeLife();
        if (lives > 0)
        {
            StartCoroutine(SpawnPlayer(RespawnDelay));
        }
    }

    IEnumerator SpawnPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);

        Instantiate(PlayerPrefab, PlayerSpawn.position, Quaternion.identity);
    }

}
