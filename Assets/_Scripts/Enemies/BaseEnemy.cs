using UnityEngine;
using System.Collections;

public class BaseEnemy : BaseAI {

    public EnemyIdentifier Type;

    [SerializeField]
    bool AutoStart = false;
    [SerializeField]
    protected GameObject ProjectilePrefab;
    [SerializeField]
    protected Transform ShotSpawn;
    [SerializeField]
    protected float ShootSpeed = 1f;
    [SerializeField]
    protected WaypointPath Path;

    protected float _shootTimer;
    protected SpriteRenderer _renderer;
    
    void Awake()
    {
        if (AutoStart)
            Invoke("Init", 1) ;
    }

    public void Init()
    {
        GetSprite();
        ChangePath();
    }

    protected virtual void Update()
    {
        CalculateMovement();

        _shootTimer -= Time.deltaTime;
        if(_shootTimer <= 0f)
        {
            _shootTimer = ShootSpeed;
            //Shoot();
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
        HandleMovement(Vector2.down);
    }

    protected void Shoot()
    {
        Instantiate(ProjectilePrefab, ShotSpawn.position, ShotSpawn.rotation);
    }

    public void ChangeShape(bool moveToNearest = false, bool changeType = true)
    {
        if (changeType)
        {
            int newType = (int)Type - 1;
            if (newType <= 0)
            {
                Destroy(gameObject);
                return;
            }
            Type = (EnemyIdentifier)newType;
        }
        GetSprite();
        ChangePath(moveToNearest);
    }

    protected void GetSprite()
    {
        Sprite sprite = null;
        switch(Type)
        {
            case EnemyIdentifier.TRIANGLE:
                sprite = Resources.Load<Sprite>("Sprites/TriangleEnemy");
                break;
            case EnemyIdentifier.SQUARE:
                sprite = Resources.Load<Sprite>("Sprites/SquareEnemy");
                break;
            case EnemyIdentifier.PENTAGON:
                sprite = Resources.Load<Sprite>("Sprites/PentagonEnemy");
                break;
            case EnemyIdentifier.HEXAGON:
                sprite = Resources.Load<Sprite>("Sprites/HexagonEnemy");
                break;
            case EnemyIdentifier.SEPTIGON:
                sprite = Resources.Load<Sprite>("Sprites/SeptigonEnemy");
                break;
            case EnemyIdentifier.OCTAGON:
                sprite = Resources.Load<Sprite>("Sprites/OctagonEnemy");
                break;
        }

        if(sprite == null)
        {
            Debug.Log("No sprite was found for enemytype " + Type.ToString());
            return;
        }

        if(_renderer == null)
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();

            if (_renderer == null)
            {
                Debug.LogError("No spriterenderer was found enemy");
                return;
            }
        }

        _renderer.sprite = sprite;
    }

    protected void ChangePath(bool moveToNearest = false)
    {
        switch(Type)
        {
            case EnemyIdentifier.TRIANGLE:
                Path.reset(WaypointPath.Shapes.none);
                break;
            case EnemyIdentifier.SQUARE:
                Path.reset(WaypointPath.Shapes.curve);
                break;
            case EnemyIdentifier.PENTAGON:
                Path.reset(WaypointPath.Shapes.infinity);
                break;
            case EnemyIdentifier.HEXAGON:
                Path.reset(WaypointPath.Shapes.envelope);
                break;
            case EnemyIdentifier.SEPTIGON:
                Path.reset(WaypointPath.Shapes.star);
                break;
            case EnemyIdentifier.OCTAGON:
                Path.reset(WaypointPath.Shapes.hourglass);
                break;
        }

        if(moveToNearest)
        {
            Path.MoveToNearestWaypoint();
        }
    }

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("BottomEdge"))
			Destroy (gameObject);
	}
}
