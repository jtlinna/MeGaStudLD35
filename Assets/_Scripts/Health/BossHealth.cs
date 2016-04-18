using UnityEngine;
using System.Collections;

public class BossHealth : BaseHealth {

    public static System.Action<float, float> OnBossDamaged;
    public static System.Action OnBossDied;

	public float[] bossMaxHealth;

	protected override void Awake ()
	{
		base.Awake();
	}

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(OnBossDamaged != null)
        {
            OnBossDamaged(_currentHealth, MaxHealth);
        }
    }

    protected override void Die()
    {
		GetComponent<BossScript> ().killBoss ();
    }

	public void SetMaxHealth(int phase){

		MaxHealth = bossMaxHealth[phase];
		_currentHealth = MaxHealth;

		if(OnBossDamaged != null)
		{
			OnBossDamaged(_currentHealth, MaxHealth);
		}
	}

}
