using UnityEngine;
using System.Collections;

public class PlayerHealth : BaseHealth {

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // TODO Update UI etc.
    }

	protected override void Die ()
	{
		GetComponent<Animator> ().SetTrigger ("Death");
	}

    public void Kill()
    {
        Die();
    }
}
