﻿using UnityEngine;
using System.Collections;

public class PowerPowerUp : PowerUp {

	[SerializeField] private int _addScore = 10;

	public override void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Player") && !other.GetComponent<Player>().PowerUp())
			manager.addScore(_addScore);
		base.OnTriggerEnter2D (other);
	}
}
