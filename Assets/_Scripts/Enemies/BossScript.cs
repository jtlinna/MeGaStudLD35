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

        if (AutoStart)
            StartCoroutine(DelayedInit());
	}

    public void Init()
    {
		StartCoroutine("preBossSequence", phase);
        Debug.Log(phase.ToString());
    }
	
	// Update is called once per frame
	void Update () {
		rotateParts (-10f, 20f, -30f);
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
		yield return new WaitForSeconds (2f);
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
		Debug.Log ("postSequenceDone");
		Destroy (gameObject);
		yield break;
	}

	public void killBoss(){
		StopAllCoroutines();
		StartCoroutine("postBossSequence", phase);
	}

    private IEnumerator DelayedInit()
    {
        yield return null;
        Init();
    }
}
