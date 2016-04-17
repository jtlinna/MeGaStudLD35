using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {

	public enum BossPhase {
		first = 1,
		second = 2,
		third = 3
	}

	public BaseHealth health;
	private Transform[] _parts;
	public BulletSequenceManager bulletManager;
	public BossPhase phase = BossPhase.first;

	public int attackSequence;
	private bool sequenceDone = false;

	// Use this for initialization
	void Awake () {

		_parts = new Transform[3];

		_parts [0] = transform.GetChild (0);
		_parts [1] = transform.GetChild (1);
		_parts [2] = transform.GetChild (2);

		StartCoroutine (preBossSequence());
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

	public IEnumerator preBossSequence () {
		yield break;
	}
	public IEnumerator sequenceOne () {
		yield break;
	}
	public IEnumerator sequenceTwo () {
		yield break;
	}
	public IEnumerator sequenceThree () {
		yield break;
	}
	public IEnumerator sequenceFour () {
		yield break;
	}
	public IEnumerator sequenceFive () {
		yield break;
	}
}
