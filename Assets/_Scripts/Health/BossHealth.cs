using UnityEngine;
using System.Collections;

public class BossHealth : BaseHealth {

    public static System.Action<float, float> OnBossDamaged;
    public static System.Action OnBossDied;

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
        base.Die();
        GameManager.BossStage++;
    }

}
