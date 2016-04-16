using UnityEngine;
using System.Collections;

public class EnemyHealth : BaseHealth {

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        // TODO Handle sprite swapping based on current health
    }
}
