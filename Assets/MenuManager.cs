using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	private Button Play, Tutorial, Quit, Back, openScore, closeScore;
	public GameObject TutorialImg, ScorePanel;

    [SerializeField]
    private LeaderboardContainer[] LeaderboardEntries;

    [SerializeField]
    private Button ToggleSoundsButton;
    [SerializeField]
    private Text ToggleSoundsText;

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

        Leaderboard leaderboard = LeaderboardManager.Instance.GetLeaderboard();

        for (int i = 0; i < LeaderboardEntries.Length; i++)
        {
            LeaderboardEntries[i].Init(leaderboard.Entries[i]);
        }
    }

    void Start()
    {
        ToggleSoundsText.text = (SoundManager.Instance.SoundsOn()) ? "SOUNDS ON" : "SOUNDS OFF";
        ToggleSoundsButton.onClick.RemoveAllListeners();
        ToggleSoundsButton.onClick.AddListener(delegate ()
        {
            ToggleSounds();
        });
    }

    public void ToggleSounds()
    {
        bool soundsOn = SoundManager.Instance.SoundsOn();
        SoundManager.Instance.ToggleSounds(!soundsOn);
        ToggleSoundsText.text = (soundsOn) ? "SOUNDS OFF" : "SOUNDS ON";
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
