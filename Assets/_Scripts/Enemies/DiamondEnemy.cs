using UnityEngine;
using System.Collections;

public class DiamondEnemy : BaseEnemy {

    protected override void CalculateMovement()
    {
        base.CalculateMovement();

        HandleMovement(Vector2.down);
    }
}
