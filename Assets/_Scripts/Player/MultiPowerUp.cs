using UnityEngine;
using System.Collections;

public class MultiPowerUp : PowerUp {

	[SerializeField] private float _addMulti = 1f;
	[SerializeField] private int _addScore = 10;

	public override void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Player") && !manager.addMultiplier(_addMulti))
			manager.addScore(_addScore);
		base.OnTriggerEnter2D (other);
	}

}
