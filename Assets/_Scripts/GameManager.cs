using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	enum States {
		playing = 1,
		paused = 2,
		menu = 3};
	
	private States currentState;
	private float gameTime;
	private ulong score;
	private int lives = 5;

	private float _lastGameTime;
	public float lastGameTime { get { return _lastGameTime; } }
	private ulong _lastScore;
	public ulong lastScore { get { return _lastScore; } }

	void Awake() {
		DontDestroyOnLoad(gameObject);
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

		score = 0;
		gameTime = 0f;
	}

	void Update () {
		if (lives < 0)
			currentState == States.menu;
		if (currentState == States.playing)
			gameTime += Time.deltaTime;
	}

	public bool addScore (ulong amount){
		if (currentState == States.playing) {
			score += amount;
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
	}

}
