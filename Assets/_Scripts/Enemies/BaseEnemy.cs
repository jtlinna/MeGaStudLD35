using UnityEngine;
using System.Collections;

public class BaseEnemy : BaseAI {

    [SerializeField]
    protected GameObject ProjectilePrefab;
    [SerializeField]
    protected Transform ShotSpawn;
    [SerializeField]
    protected float ShootSpeed = 1f;

    protected float _shootTimer;

    protected virtual void Update()
    {
        CalculateMovement();

        _shootTimer -= Time.deltaTime;
        if(_shootTimer <= 0f)
        {
            _shootTimer = ShootSpeed;
            Shoot();
        }
    }



    public override void HandleMovement(Vector3 movement)
    {
        base.HandleMovement(movement);

        Vector3 normalizedMovement = movement.normalized;
        transform.position += normalizedMovement * MoveSpeed * Time.deltaTime;
    }

    protected virtual void CalculateMovement()
    {

    }

    public void SwapSprite(float health)
    {

    }

    protected void Shoot()
    {
        Instantiate(ProjectilePrefab, ShotSpawn.position, ShotSpawn.rotation);
    }
}
