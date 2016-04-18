using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : BaseAI {

    public static System.Action OnLastEnemyRemoved;

    private static List<BaseEnemy> ActiveEnemies = new List<BaseEnemy>();

    public static void AddEnemy(BaseEnemy enemy)
    {
        ActiveEnemies.Add(enemy);
    }

    public static void RemoveEnemy(BaseEnemy enemy)
    {
        if (ActiveEnemies.Contains(enemy))
            ActiveEnemies.Remove(enemy);

        if(ActiveEnemies.Count < 1 && OnLastEnemyRemoved != null)
        {
            OnLastEnemyRemoved();
        }
    }

    public static void RemoveAllEnemies(bool destroyObject = false)
    {
        if (destroyObject)
        {
            for (int i = ActiveEnemies.Count - 1; i >= 0; i--)
            {
                ActiveEnemies[i].Die();
            }
        }
    }
    
    public EnemyIdentifier Type;


    [SerializeField]
    bool AutoStart = false;
    [SerializeField]
    protected Transform[] ShotSpawns;
    [SerializeField]
    protected WaypointPath Path;
	[SerializeField]
	protected BulletSequenceManager BulletManager;
    [SerializeField]
    protected GameObject ScorePowerUpPrefab;

    protected float _shootTimer;
    protected SpriteRenderer _renderer;
    protected PowerUpIdentifier _additionalPowerUp;

    protected Transform _graphics;
    
    void Awake()
    {
#if !UNITY_EDITOR
        AutoStart = false;
#endif
        if (AutoStart)
			StartCoroutine (delayedInit ());

        AddEnemy(this);
        _graphics = transform.FindChild("Graphics");

        ShotSpawns = new Transform[_graphics.childCount];
		for (int i = 0; i < ShotSpawns.Length; i++) {
			ShotSpawns [i] = _graphics.GetChild (i);
		}
    }

    void OnDestroy()
    {
        RemoveEnemy(this);
    }

	private IEnumerator delayedInit() {
		yield return null;
		Init (PowerUpIdentifier.NONE);
	}

    public void Init(PowerUpIdentifier additionalPowerUp)
    {
        _additionalPowerUp = additionalPowerUp;
        GetSprite();
        ChangePath();
		ChangeShotSpawns ();
		Shoot ();
    }

    protected virtual void Update()
    {
        CalculateMovement();
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
		BulletManager.stopSpawning ();
		switch (Type) {
		case EnemyIdentifier.TRIANGLE:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletType.TRIANGLE, 3, 0f, 0.2f, 1f));
			break;
		case EnemyIdentifier.SQUARE:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletType.SQUARE, 1, 0f, 0f, (2f/3f)));
			break;
		case EnemyIdentifier.PENTAGON:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletType.PENTAGON, 1, 0f, 0f, 1f));
			break;
		case EnemyIdentifier.HEXAGON:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletType.HEXAGON, 1, 0.1f, 0f, 0f));
			break;
		case EnemyIdentifier.SEPTIGON:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletType.SEPTIGON, 2, 0f, 0.5f, 2f));
			break;
		case EnemyIdentifier.OCTAGON:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletType.OCTAGON, 8, 0f, (1f / 8f), 3f));
			break;
		}
    }

    public void ChangeShape(bool goToNearest = false, bool changeType = true)
    {
        if (changeType)
        {
            int newType = (int)Type - 1;
            if (newType < 3)
            {
                Die();
                return;
            }
            Type = (EnemyIdentifier)newType;
        }
        GetSprite ();
        ChangePath (goToNearest);
		ChangeShotSpawns ();
		Shoot ();

        StopCoroutine("RotateSquare");
        if(Type == EnemyIdentifier.SQUARE)
        {
            StartCoroutine("RotateSquare");
        }
        else
        {
            _graphics.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
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

    protected void ChangePath(bool goToNearest = false)
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
        if (goToNearest)
            Path.MoveToNearestWaypoint();
    }

	protected void ChangeShotSpawns()
	{
		switch(Type)
		{
		case EnemyIdentifier.TRIANGLE:
			setShotSpawns (3);
			break;
		case EnemyIdentifier.SQUARE:
			setShotSpawns (4);
			break;
		case EnemyIdentifier.PENTAGON:
			setShotSpawns (5);
			break;
		case EnemyIdentifier.HEXAGON:
			setShotSpawns (6);
			break;
		case EnemyIdentifier.SEPTIGON:
			setShotSpawns (7);
			break;
		case EnemyIdentifier.OCTAGON:
			setShotSpawns (8);
			break;
		}
	}

	private void setShotSpawns(int corners)
	{
		for (int i = 0; i < ShotSpawns.Length; i++) {
			ShotSpawns [i].gameObject.SetActive (i < corners ? true : false);
		}

		int z = 0;
		foreach (Transform spawn in ShotSpawns) {
			spawn.localPosition = Vector3.zero;
			spawn.localRotation = Quaternion.Euler(Vector3.zero);
			if (corners != 5) spawn.Rotate (new Vector3 (0f, 0f, (360f / corners) * z));
			if (corners != 5) spawn.localPosition += spawn.up * 2f;
			z++;
		}
		z = 0;
	}

    public void DropPowerUp()
    {
        if(_additionalPowerUp == PowerUpIdentifier.NONE)
        {
            Instantiate(ScorePowerUpPrefab, transform.position, Quaternion.identity);
            return;
        }

        string path = "PowerUps/";
        switch(_additionalPowerUp)
        {
            case PowerUpIdentifier.LIFE:
                path += "LifePowerUp";
                break;
            case PowerUpIdentifier.SCORE_MULTIPLIER:
                path += "MultiPowerUp";
                break;
            case PowerUpIdentifier.WEAPON:
                path += "WeaponPowerUp";
                break;
            case PowerUpIdentifier.FIRERATE:
                path += "RofPowerUp";
                break;
            case PowerUpIdentifier.BOMB:
                path += "BombPowerUp";
                break;
        }

        GameObject powerUp = Resources.Load<GameObject>(path);
        if(powerUp == null)
        {
            Debug.LogError("No prefab was found for powerup " + _additionalPowerUp.ToString());
            return;
        }

        Instantiate(powerUp, _graphics.position, Quaternion.identity);
    }

    IEnumerator RotateSquare()
    {
        while(true)
        {
            _graphics.transform.Rotate(new Vector3(0f, 0f, 90f * Time.deltaTime));
            yield return null;
        }
    }

    public void Die()
    {
        DropPowerUp();
        Destroy(gameObject);
    }

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("BottomEdge"))
			Destroy (gameObject);
	}
}
