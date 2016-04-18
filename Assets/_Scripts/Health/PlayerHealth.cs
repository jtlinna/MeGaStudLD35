using UnityEngine;
using System.Collections;

public class PlayerHealth : BaseHealth {

    [SerializeField]
    Player player;

    public override void TakeDamage(float damage)
    {     
        if (!player.CanBeDamaged())
            return;

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
