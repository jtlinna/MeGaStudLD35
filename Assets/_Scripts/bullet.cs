using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	[SerializeField] private float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, speed);
	}
}
