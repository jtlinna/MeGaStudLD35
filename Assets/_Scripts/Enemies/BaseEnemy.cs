using UnityEngine;
using System.Collections;

public class BaseEnemy : BaseAI {

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
    
    void Awake()
    {
		if (AutoStart)
			StartCoroutine (delayedInit ());

		ShotSpawns = new Transform[transform.FindChild("Graphics").childCount];
		for (int i = 0; i < ShotSpawns.Length; i++) {
			ShotSpawns [i] = transform.FindChild ("Graphics").GetChild (i);
		}
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

		if (Type == EnemyIdentifier.SQUARE)
			transform.FindChild ("Graphics").transform.Rotate (new Vector3 (0f, 0f, 90f * Time.deltaTime));
		else transform.FindChild ("Graphics").transform.rotation = Quaternion.Euler (Vector3.zero);

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
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletSequenceManager.bulletIdentifier.triangleBullet, 3, 0f, 0.2f, 1f));
			break;
		case EnemyIdentifier.SQUARE:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletSequenceManager.bulletIdentifier.squareBullet, 1, 0f, 0f, (2f/3f)));
			break;
		case EnemyIdentifier.PENTAGON:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletSequenceManager.bulletIdentifier.pentaBullet, 1, 0f, 0f, 1f));
			break;
		case EnemyIdentifier.HEXAGON:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletSequenceManager.bulletIdentifier.hexBullet, 1, 0.1f, 0f, 0f));
			break;
		case EnemyIdentifier.SEPTIGON:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletSequenceManager.bulletIdentifier.septiBullet, 2, 0f, 0.5f, 2f));
			break;
		case EnemyIdentifier.OCTAGON:
			BulletManager.StartCoroutine (BulletManager.spawnBullets (ShotSpawns, BulletSequenceManager.bulletIdentifier.octaBullet, 8, 0f, (1f / 8f), 3f));
			break;
		}
    }

    public void ChangeShape(bool goToNearest = false, bool changeType = true)
    {
        if (changeType)
        {
            int newType = (int)Type - 1;
            if (newType <= 0)
            {
                DropPowerUp();
                Destroy(gameObject);
                return;
            }
            Type = (EnemyIdentifier)newType;
        }
        GetSprite ();
        ChangePath (goToNearest);
		ChangeShotSpawns ();
		Shoot ();
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
            case PowerUpIdentifier.HEALTH:
                path += "LifePowerUp";
                break;
            case PowerUpIdentifier.SCORE_MULTIPLIER:
                path += "MultiPowerUp";
                break;
            case PowerUpIdentifier.WEAPON:
                path += "WeaponPowerUp";
                break;
        }

        GameObject powerUp = Resources.Load<GameObject>(path);
        if(powerUp == null)
        {
            Debug.LogError("No prefab was found for powerup " + _additionalPowerUp.ToString());
            return;
        }

        Instantiate(powerUp, transform.position, Quaternion.identity);
    }

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("BottomEdge"))
			Destroy (gameObject);
	}
}
