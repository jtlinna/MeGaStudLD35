using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {

	public enum BossPhase {
		first = 1,
		second = 2,
		third = 3
	}
    public bool AutoStart;
	public BaseHealth health;
	private Transform[] _parts;
	public BulletSequenceManager bulletManager;
	public BossPhase phase = BossPhase.first;
	public Collider2D bossCollider;
	public static System.Action OnBossDied;
	private bool thirdPhaseDead = false;
	private float spawnerPositions = 0f;

	private float sequenceTimer = 0f;

	private float coreSpinSpeed = 10f;

	public int[] shotAmountOne, shotAmountTwo, shotAmountFour, shotAmountFive;
	public float[] sequenceTimeOne;

	private Transform[] sOneSpawns, sTwoSpawns, sThreeSpawns, sFourSpawns, sFiveSpawns;

	//public int attackSequence;
	private bool sequenceDone = false;

	// Use this for initialization
	void Awake () {
		bossCollider = GetComponent<Collider2D> ();
		bossCollider.enabled = false;

		_parts = new Transform[3];

		_parts [0] = transform.GetChild (0);
		_parts [1] = transform.GetChild (1);
		_parts [2] = transform.GetChild (2);

		sOneSpawns = new Transform[shotAmountOne[shotAmountOne.Length - 1]];

        if (AutoStart)
            StartCoroutine(DelayedInit());
	}

    public void Init()
    {
		for (int i = 0; i < shotAmountOne[shotAmountOne.Length - 1]; i++){
			sOneSpawns [i] = transform.FindChild ("Core").FindChild ("SequenceOne").GetChild (i);
			if (i < shotAmountOne [(int)phase - 1]) {
				sOneSpawns [i].Rotate (new Vector3(0f,0f,(360f / shotAmountOne[(int)phase - 1]) * i));
				sOneSpawns [i].localPosition = sOneSpawns [i].forward * spawnerPositions;
				sOneSpawns [i].gameObject.SetActive (true);
			} else
				sOneSpawns [i].gameObject.SetActive (false);
		}
		GetComponent<Animator> ().SetInteger ("phase", (int)phase);
		StartCoroutine("preBossSequence", phase);
        Debug.Log(phase.ToString());
    }
	
	// Update is called once per frame
	void Update () {
		rotateParts (coreSpinSpeed, coreSpinSpeed - 40f,  coreSpinSpeed * 3f);
		sequenceTimer += Time.deltaTime;
	}

	private void rotateParts (float coreSpeed, float midSpeed, float baseSpeed) {
		_parts [0].Rotate (new Vector3(0f,0f, coreSpeed * Time.deltaTime));
		_parts [1].Rotate (new Vector3(0f,0f, midSpeed * Time.deltaTime));
		_parts [2].Rotate (new Vector3(0f,0f, baseSpeed * Time.deltaTime));
	}

	private void moveBoss (float xPerSec, float yPerSec) {
		transform.position = new Vector2 (transform.position.x + (xPerSec * Time.deltaTime), transform.position.y + (yPerSec * Time.deltaTime));
	}

	public IEnumerator preBossSequence (BossPhase phase) {
		while (transform.position.y > 20f) {
			moveBoss (0f, -2f);
			yield return new WaitForEndOfFrame ();
		}
		bossCollider.enabled = true;
		StartCoroutine ("sequenceFive", phase);
		Debug.Log ("PreSequenceDone");
		yield break;
	}
	public IEnumerator sequenceOne (BossPhase phase) {

		coreSpinSpeed = coreSpinSpeed * 3f;
		sequenceTimer = 0f;

		if (transform.position != new Vector3 (0f, 20f)) {
			//Move me better bitch
			transform.position = new Vector2 (0f, 20f);
		}

		bulletManager.StartCoroutine (bulletManager.spawnBullets(sOneSpawns, BulletSequenceManager.bulletIdentifier.octaBullet, 1, 0f, 0f, 1f/6f));

		while (sequenceTimeOne[(int)phase - 1] > sequenceTimer) {
			yield return new WaitForEndOfFrame ();
		}

		bulletManager.stopSpawning ();
		
		coreSpinSpeed = coreSpinSpeed / 3f;

		Debug.Log ("SequenceOneDone");
		yield break;
	}

	public IEnumerator sequenceTwo (BossPhase phase) {
		yield return sequenceOne (phase);
		yield return new WaitForSeconds (2f);
		Debug.Log ("SequenceTwoDone");
		yield break;
	}
	public IEnumerator sequenceThree (BossPhase phase) {
		yield return sequenceTwo (phase);
		yield return new WaitForSeconds (2f);
		Debug.Log ("SequenceThreeDone");
		yield break;
	}
	public IEnumerator sequenceFour (BossPhase phase) {
		yield return sequenceThree (phase);
		yield return new WaitForSeconds (2f);
		Debug.Log ("SequenceFourDone");
		yield break;
	}
	public IEnumerator sequenceFive (BossPhase phase) {
		yield return sequenceFour (phase);
		yield return new WaitForSeconds (2f);
		Debug.Log ("SequenceFiveDone");
		StartCoroutine ("sequenceFive", phase);
		yield break;
	}

	public IEnumerator postBossSequence(BossPhase phase) {
		bossCollider.enabled = false;
		if (phase != BossPhase.third) {
			bool bossZero = transform.position.x == 0f ? true : false;
			bool bossLeft = transform.position.x < 0f ? true : false;
			while (transform.position.y < 40f) {
				if (!bossZero && (bossLeft && transform.position.x < 0f))
					moveBoss (-1f, 0f);
				else if (!bossZero && (!bossLeft && transform.position.x > 0f))
					moveBoss (1f, 0f);
				else
					transform.position = new Vector2 (0f, transform.position.y);
				moveBoss (0f, 2f);
				yield return new WaitForEndOfFrame ();
			}
		} else {
			while (!thirdPhaseDead) {
				yield return new WaitForEndOfFrame ();
			}

		}
		Debug.Log ("postSequenceDone");

		if (OnBossDied != null) {
			OnBossDied ();
		}

		Destroy (gameObject);
		yield break;
	}

	public void killBoss(){
		StopAllCoroutines();
		GetComponent<Animator> ().SetTrigger ("death");
		StartCoroutine("postBossSequence", phase);
	}

	public void bossDeadFromAnimation() {
		thirdPhaseDead = true;
	}

    private IEnumerator DelayedInit()
    {
        yield return null;
        Init();
    }
}
