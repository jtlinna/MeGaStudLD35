using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static System.Action OnPlayerDied;

	[SerializeField] private float velocity = 10f;
	private UIManager UI;
	private Rigidbody2D rigidBody;
	private Collider2D col;
	private Vector2 inputData;
	private Transform muzzle;

	public BaseHealth health;
	public GameObject bulletPrefab;
	public int shotType = 1;
	private float rateOfFire = 5f;
	private float maxRateOfFire = 30f;

	private float rofTimer = 0f;
	private float invulnTime = 3f;
	private float _lastFrameAxis = 0f;
	private bool control = true;

	private float upBound, downBound, leftBound, rightBound;

	// Use this for initialization
	public virtual void Awake () {
		invulnTime = 3f;
		UI = FindObjectOfType<UIManager> ();
		StartCoroutine (getBound());
		rigidBody = GetComponent<Rigidbody2D> ();
		col = GetComponent<CircleCollider2D> ();
		col.enabled = true;
		muzzle = transform.FindChild ("Muzzle");
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
    }
	
	// Update is called once per frame
	public virtual void Update () {
		if (control) {
			inputData = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			move (inputData);

			if (rofTimer < 1f/rateOfFire) rofTimer += Time.deltaTime;

			if (Input.GetAxisRaw ("Fire") > 0f) {
				if (rofTimer >= 1f/rateOfFire) {
					shoot (shotType);
					rofTimer = 0f;
				}
			}
			if (Input.GetAxisRaw ("Bomb") > 0f && _lastFrameAxis == 0f)
				bombDaShit ();
			_lastFrameAxis = Input.GetAxisRaw ("Bomb");
		}

		if (invulnTime > 0f) {
			invulnTime -= Time.deltaTime;
		} else {
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
		}
	}

	private void move(Vector2 movement) {
		rigidBody.velocity = movement.normalized * velocity * (Input.GetAxisRaw ("Focus") <= 0f ? 1f : 0.4f);

		if (rigidBody.velocity.x < 0f && transform.position.x <= leftBound)
			rigidBody.velocity = new Vector2 (0f, rigidBody.velocity.y);
		if (rigidBody.velocity.x > 0f && transform.position.x >= rightBound)
			rigidBody.velocity = new Vector2 (0f, rigidBody.velocity.y);
		if (rigidBody.velocity.y > 0f && transform.position.y >= upBound)
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, 0f);
		if (rigidBody.velocity.y < 0f && transform.position.y <= downBound)
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, 0f);
	}

	private void shoot(int type = 1) {
		if (type == 1) {
			Instantiate (bulletPrefab, muzzle.position, Quaternion.identity);
		} else if (type == 2) {
			Instantiate (bulletPrefab, muzzle.position, Quaternion.identity);
			Instantiate (bulletPrefab, new Vector2(muzzle.position.x + (Input.GetAxisRaw ("Focus") <= 0f ? 0f : 0.5f), muzzle.position.y), Quaternion.Euler (0f, 0f, Input.GetAxisRaw ("Focus") <= 0f ? 15f : 0f));
			Instantiate (bulletPrefab, new Vector2(muzzle.position.x - (Input.GetAxisRaw ("Focus") <= 0f ? 0f : 0.5f), muzzle.position.y), Quaternion.Euler (0f, 0f, Input.GetAxisRaw ("Focus") <= 0f ? -15f : 0f));
		} else if (type == 3) {
			Instantiate (bulletPrefab, muzzle.position, Quaternion.identity);
			Instantiate (bulletPrefab, new Vector2(muzzle.position.x + (Input.GetAxisRaw ("Focus") <= 0f ? 0f : 0.5f), muzzle.position.y), Quaternion.Euler (0f, 0f, Input.GetAxisRaw ("Focus") <= 0f ? 15f : 0f));
			Instantiate (bulletPrefab, new Vector2(muzzle.position.x - (Input.GetAxisRaw ("Focus") <= 0f ? 0f : 0.5f), muzzle.position.y), Quaternion.Euler (0f, 0f, Input.GetAxisRaw ("Focus") <= 0f ? -15f : 0f));
			Instantiate (bulletPrefab, muzzle.position, Quaternion.Euler (0f, 0f, Input.GetAxisRaw ("Focus") <= 0f ? 7.5f : 4.5f));
			Instantiate (bulletPrefab, muzzle.position, Quaternion.Euler (0f, 0f, Input.GetAxisRaw ("Focus") <= 0f ? -7.5f : -4.5f));
		}
	}

	private void bombDaShit() {
		GameManager.Instance.useBomb ();
	}

	public bool PowerUp () {
		if (shotType < 3){
			shotType++;
			GetComponent<Animator> ().SetInteger ("PowerStage", shotType);
			return true;
		}
		else
			return false;
	}

	public bool RofUp () {
		if (rateOfFire < maxRateOfFire) {
			rateOfFire += 2.5f;
			return true;
		} else {
			rateOfFire = maxRateOfFire;
			return false;
		}
	}

	public void toggleControls () {
		if (control) {
			control = false;
			col.enabled = false;
			rigidBody.velocity = Vector2.zero;
		} else
			control = true;
	}

	public void destroyPlayer () {
        if(OnPlayerDied != null)
        {
            OnPlayerDied();
        }
		Destroy (gameObject);
	}
	
    public bool CanBeDamaged()
    {
        return invulnTime <= 0f;
    }

	private IEnumerator getBound() {
		yield return null;
		upBound = UI.GetCorner (UICorners.TOP_LEFT).y;
		leftBound = UI.GetCorner (UICorners.TOP_LEFT).x;
		downBound = UI.GetCorner (UICorners.BOTTOM_RIGHT).y;
		rightBound = UI.GetCorner (UICorners.BOTTOM_RIGHT).x;
	}

//	public void resetPlayer () {
//		
//		shotType = 1;
//	}
}