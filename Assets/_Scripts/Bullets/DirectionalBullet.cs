using UnityEngine;
using System.Collections;

public class DirectionalBullet : BaseBullet {
    
    private Vector3 _movementVector;
	private bool targetAquired = false;

	void Awake () {
		_movementVector = transform.up;
	}

    protected override void DoMovement()
    {
        base.DoMovement();
		if (!targetAquired && GameObject.FindGameObjectWithTag("Player") != null) SetTarget (GameObject.FindGameObjectWithTag("Player"));
		transform.position += _movementVector * MoveSpeed * Time.deltaTime;
    }

    public void SetTarget(GameObject target)
    {
        _movementVector = target.transform.position - transform.position;
        _movementVector = _movementVector.normalized;
		targetAquired = true;
    }
}
