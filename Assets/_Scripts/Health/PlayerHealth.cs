using UnityEngine;
using System.Collections;

public class PlayerHealth : BaseHealth {

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // TODO Update UI etc.
    }
}
