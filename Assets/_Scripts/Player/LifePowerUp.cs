using UnityEngine;
using System.Collections;

public class LifePowerUp : PowerUp {

	public override void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Player"))
			manager.addLife ();
		base.OnTriggerEnter2D (other);
	}

}
