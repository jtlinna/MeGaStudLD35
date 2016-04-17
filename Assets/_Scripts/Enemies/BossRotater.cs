using UnityEngine;
using System.Collections;

public class BossRotater : MonoBehaviour {

	private Transform[] _parts;

	// Use this for initialization
	void Start () {
		_parts = new Transform[3];

		_parts [0] = transform.GetChild (0);
		_parts [1] = transform.GetChild (1);
		_parts [2] = transform.GetChild (2);
	}
	
	// Update is called once per frame
	void Update () {
		_parts [0].Rotate (new Vector3(0f,0f, -5f * Time.deltaTime));
		_parts [1].Rotate (new Vector3(0f,0f, 25f * Time.deltaTime));
		_parts [2].Rotate (new Vector3(0f,0f, -20f * Time.deltaTime));
	}
}
