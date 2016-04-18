using UnityEngine;
using System.Collections;

public class DirectionalBullet : BaseBullet {
    
    private Vector3 _movementVector;
	private bool targetAquired = false;
	private GameObject Player;

    protected override void OnEnable ()
    {
		if (GameObject.FindGameObjectWithTag ("Player") != null)
			Player = GameObject.FindGameObjectWithTag ("Player");
        base.OnEnable();
        _movementVector = transform.up;
	}

    protected override void DoMovement()
    {
        base.DoMovement();

		if (!targetAquired && Player != null) SetTarget (Player);
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
