using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private Button Play, Tutorial, Quit, Back;
	public GameObject TutorialImg;

	public string GameSceneName;

	// Use this for initialization
	void Awake () {
		Play = GameObject.Find ("Play").GetComponent<Button> ();
		Tutorial = GameObject.Find ("HowToPlay").GetComponent<Button> ();
		Quit = GameObject.Find ("Quit").GetComponent<Button> ();
		Back = GameObject.Find ("Back").GetComponent<Button> ();
		TutorialImg.SetActive (false);

		Play.onClick.AddListener (() => loadGame());
		Tutorial.onClick.AddListener (() => openTutorialPanel());
		Quit.onClick.AddListener (() => quitGame());
		Back.onClick.AddListener (() => closeTutorialPanel());
	}

	private void loadGame () {
		SceneManager.LoadScene (GameSceneName); 
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
