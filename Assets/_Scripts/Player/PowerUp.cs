﻿using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	protected GameManager manager;
	[SerializeField] private float MoveSpeed = 3f;

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	// Update is called once per frame
	void Update () {
		transform.position += -transform.up.normalized * MoveSpeed * Time.deltaTime;
	}

	public virtual void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Player") || other.CompareTag("BottomEdge")) {
			Destroy (gameObject);
		}
	}
}
