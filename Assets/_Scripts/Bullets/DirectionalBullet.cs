using UnityEngine;
using System.Collections;

public class DirectionalBullet : BaseBullet {
    
    private Vector3 _movementVector;

    protected override void DoMovement()
    {
        base.DoMovement();
        transform.position += _movementVector * Time.deltaTime;
    }

    public void SetTarget(GameObject target)
    {
        _movementVector = target.transform.position - transform.position;
        _movementVector = _movementVector.normalized;
    }
}
