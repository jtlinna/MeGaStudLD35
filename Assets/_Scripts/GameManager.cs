﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	enum States {
		playing = 1,
		paused = 2,
		menu = 3};
	
	private States currentState;
	[SerializeField] private float gameTime;
	[SerializeField] private ulong score;
	private float maxMultiplier = 20f;
	[SerializeField] private float multiplier = 1f;
	[SerializeField] private int lives = 5;

	private float _lastGameTime;
	public float lastGameTime { get { return _lastGameTime; } }
	private ulong _lastScore;
	public ulong lastScore { get { return _lastScore; } }

	void Awake() {
		DontDestroyOnLoad(gameObject);

		StartGame ();
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

}
