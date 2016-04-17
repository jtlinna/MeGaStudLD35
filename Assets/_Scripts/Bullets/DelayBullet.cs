using UnityEngine;
using System.Collections;

public class DelayBullet : BaseBullet {
    
    private Vector3 _movementVector;
	public float timeToStop = 2f, stopTime = 2f;
	private float timer = 0f;
	private float currentMoveSpeed;
	private bool targetAquired = false;

	protected override void Awake() {
        base.Awake();
		_movementVector = transform.up;
		timer = 0f;
		currentMoveSpeed = MoveSpeed;
	}

    protected override void DoMovement()
    {
        base.DoMovement();
		timer += Time.deltaTime;
		if (timer < timeToStop) {
			currentMoveSpeed = MoveSpeed * 0.5f;
		} else if (timer >= timeToStop && timer < (stopTime + timeToStop)) {
			currentMoveSpeed = 0f;
		} else if (timer >= (stopTime + timeToStop)) {
			if (!targetAquired)
				if (GameObject.Find ("Player") != null) 
					SetTarget (GameObject.Find ("Player"));
			currentMoveSpeed = MoveSpeed * 2f;
		}
		transform.position += _movementVector * Time.deltaTime * currentMoveSpeed;
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
