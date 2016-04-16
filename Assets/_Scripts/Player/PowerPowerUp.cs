using UnityEngine;
using System.Collections;

public class PowerPowerUp : PowerUp {

	public override void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Player"))
			other.GetComponent<Player>().PowerUp() ? : GameObject.Find("GameManager").GetComponent<GameManager>().addScore(10) ;
		base.OnTriggerEnter2D (other);
	}
}
