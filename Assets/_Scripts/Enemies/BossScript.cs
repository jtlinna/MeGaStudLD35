﻿using UnityEngine;
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
	private float spawnerPositions = 1f;
	public float bossSpeed = 8f;
	public bool bossGoRight = true;

	private Transform wpDefault, wpMiddle, wpTopLeft, wpTopRight, wpSpawn;

	private float sequenceTimer = 0f;

	private float coreSpinSpeed = 10f;
	private float coreSpinSpeedDefault;

	public int[] shotAmountOne, shotAmountTwo, shotAmountFour;
	public float[] sequenceTimeOne, sequenceTimeTwo;
	public int[] tripAmout;
	private int tripCounter;

	private Transform[] sOneSpawns, sTwoSpawns, sThreeSpawn, sFourSpawns, sFiveSpawn;

	//public int attackSequence;
	private bool sequenceDone = false;

	// Use this for initialization
	void Awake () {
		wpDefault = GameObject.Find("wpDefault").transform;
		wpMiddle = GameObject.Find("wpMiddle").transform;
		wpTopLeft = GameObject.Find("wpTopLeft").transform;
		wpTopRight = GameObject.Find("wpTopRight").transform;
		wpSpawn = GameObject.Find("wpSpawn").transform;

		coreSpinSpeedDefault = coreSpinSpeed;
		bossCollider = GetComponent<Collider2D> ();
		bossCollider.enabled = false;

		_parts = new Transform[3];

		_parts [0] = transform.GetChild (0);
		_parts [1] = transform.GetChild (1);
		_parts [2] = transform.GetChild (2);

		sOneSpawns = new Transform[shotAmountOne[shotAmountOne.Length - 1]];
		sThreeSpawn = new Transform[1];
		sFiveSpawn = new Transform[1];

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

		sThreeSpawn[0] = transform.FindChild ("SequenceThreeNFive");

		sThreeSpawn[0].rotation = Quaternion.Euler(new Vector3(0f,0f,180f));

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

	public IEnumerator preBossSequence (BossPhase phase) {
		while (transform.position != wpDefault.position) {
			transform.position = Vector2.MoveTowards (transform.position, wpDefault.position, (bossSpeed * 0.25f) * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
		bossCollider.enabled = true;
		StartCoroutine ("sequenceFive", phase);
		Debug.Log ("PreSequenceDone");
		yield break;
	}

	public IEnumerator sequenceOne (BossPhase phase) {

		while (transform.position != wpDefault.position) {
			transform.position = Vector2.MoveTowards (transform.position, wpDefault.position, bossSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		coreSpinSpeed = coreSpinSpeedDefault * 3f;
		sequenceTimer = 0f;

		bulletManager.StartCoroutine (bulletManager.spawnBullets(sOneSpawns, BulletSequenceManager.bulletIdentifier.octaBullet, 1, 0f, 0f, 1f/6f));

		while (sequenceTimeOne[(int)phase - 1] > sequenceTimer) {
			yield return new WaitForEndOfFrame ();
		}

		bulletManager.stopSpawning ();
		
		coreSpinSpeed = coreSpinSpeedDefault;

		Debug.Log ("SequenceOneDone");
		yield break;
	}

	public IEnumerator sequenceTwo (BossPhase phase) {
		yield return sequenceOne (phase);

		while (transform.position != wpMiddle.position) {
			transform.position = Vector2.MoveTowards (transform.position, wpMiddle.position, bossSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		sequenceTimer = 0f;

//		while (sequenceTimeOne[(int)phase - 1] > sequenceTimer) {
//			yield return new WaitForEndOfFrame ();
//		}

		yield return new WaitForSeconds (2f);
		Debug.Log ("SequenceTwoDone");
		yield break;
	}

	public IEnumerator sequenceThree (BossPhase phase) {
		yield return sequenceTwo (phase);

		while (transform.position != wpTopLeft.position) {
			transform.position = Vector2.MoveTowards (transform.position, wpTopLeft.position, bossSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		tripCounter = 0;

		bulletManager.StartCoroutine (bulletManager.spawnBullets(sThreeSpawn, BulletSequenceManager.bulletIdentifier.octaBullet, 1, 0f, 0f, 1f/10f));

		while (tripAmout[(int)phase - 1] > tripCounter) {
			if (bossGoRight)
				transform.position = Vector2.MoveTowards (transform.position, wpTopRight.position, bossSpeed * 2f * Time.deltaTime);
			else 
				transform.position = Vector2.MoveTowards (transform.position, wpTopLeft.position, bossSpeed * 2f * Time.deltaTime);

			if (transform.position == wpTopRight.position) {
				bossGoRight = false;
				tripCounter++;
			}
			if (transform.position == wpTopLeft.position){
				bossGoRight = true;
				tripCounter++;
			}
			
			yield return new WaitForEndOfFrame ();
		}

		bulletManager.stopSpawning ();

		yield return new WaitForSeconds (2f);
		Debug.Log ("SequenceThreeDone");
		yield break;
	}

	public IEnumerator sequenceFour (BossPhase phase) {
		yield return sequenceThree (phase);

		while (transform.position != wpDefault.position) {
			transform.position = Vector2.MoveTowards (transform.position, wpDefault.position, bossSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		yield return new WaitForSeconds (2f);
		Debug.Log ("SequenceFourDone");
		yield break;
	}

	public IEnumerator sequenceFive (BossPhase phase) {
		yield return sequenceFour (phase);

		while (transform.position != wpTopLeft.position) {
			transform.position = Vector2.MoveTowards (transform.position, wpTopLeft.position, bossSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		tripCounter = 0;

		bulletManager.StartCoroutine (bulletManager.spawnBullets(sThreeSpawn, BulletSequenceManager.bulletIdentifier.triangleBullet, 1, 0f, 0f, 1f/10f));

		while (tripAmout[(int)phase - 1] > tripCounter) {
			if (bossGoRight)
				transform.position = Vector2.MoveTowards (transform.position, wpTopRight.position, bossSpeed * 2f * Time.deltaTime);
			else 
				transform.position = Vector2.MoveTowards (transform.position, wpTopLeft.position, bossSpeed * 2f * Time.deltaTime);

			if (transform.position == wpTopRight.position) {
				bossGoRight = false;
				tripCounter++;
			}
			if (transform.position == wpTopLeft.position){
				bossGoRight = true;
				tripCounter++;
			}

			yield return new WaitForEndOfFrame ();
		}

		bulletManager.stopSpawning ();

		yield return new WaitForSeconds (2f);
		Debug.Log ("SequenceFiveDone");
		StartCoroutine ("sequenceFive", phase);
		yield break;
	}

	public IEnumerator postBossSequence(BossPhase phase) {
		bossCollider.enabled = false;
		if (phase != BossPhase.third) {
			while (transform.position != wpSpawn.position) {
				transform.position = Vector2.MoveTowards (transform.position, wpSpawn.position, bossSpeed * 0.25f * Time.deltaTime);
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
