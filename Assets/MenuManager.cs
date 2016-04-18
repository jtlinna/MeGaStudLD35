using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private Button Play, Tutorial, Quit, Back, openScore, closeScore;
	public GameObject TutorialImg, ScorePanel;

	public string GameSceneName;

	// Use this for initialization
	void Awake () {
		Play = GameObject.Find ("Play").GetComponent<Button> ();
		Tutorial = GameObject.Find ("HowToPlay").GetComponent<Button> ();
		Quit = GameObject.Find ("Quit").GetComponent<Button> ();
		Back = GameObject.Find ("Back").GetComponent<Button> ();
		openScore = GameObject.Find ("Scores").GetComponent<Button> ();
		closeScore = GameObject.Find ("closeScores").GetComponent<Button> ();
		TutorialImg.SetActive (false);
		ScorePanel.SetActive (false);

		Play.onClick.AddListener (() => loadGame());
		Tutorial.onClick.AddListener (() => openTutorialPanel());
		Quit.onClick.AddListener (() => quitGame());
		Back.onClick.AddListener (() => closeTutorialPanel());
		openScore.onClick.AddListener (() => openScores ());
		closeScore.onClick.AddListener (() => closeScores ());
	}

	private void loadGame () {
		SceneManager.LoadScene (GameSceneName); 
	}

	private void openScores () {
		ScorePanel.SetActive(true);
	}

	private void closeScores () {
		ScorePanel.SetActive(false);
	}

	private void openTutorialPanel () {
		TutorialImg.SetActive(true);
	}

	private void closeTutorialPanel () {
		TutorialImg.SetActive(false);
	}

	private void quitGame () {
		Application.Quit();
	}
}
