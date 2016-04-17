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
		float angle = Mathf.Atan2(_movementVector.y, _movementVector.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    public void SetTarget(GameObject target)
    {
		_movementVector = target.transform.position - transform.position;
		_movementVector = _movementVector.normalized;
		targetAquired = true;
    }
}
