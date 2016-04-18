using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static System.Action OnPlayerDied;

	[SerializeField] private float velocity = 10f;
	private Rigidbody2D rigidBody;
	private Collider2D col;
	private Vector2 inputData;
	private Transform muzzle;

	public BaseHealth health;
	public GameObject bulletPrefab;
	public int shotType = 1;
	public float rateOfFire = 0.5f;
	private float rofTimer = 0f;
	private float invulnTime = 3f;
	private float _lastFrameAxis = 0f;
	private bool control = true;

	// Use this for initialization
	public virtual void Awake () {
		rigidBody = GetComponent<Rigidbody2D> ();
		col = GetComponent<Collider2D> ();
		muzzle = transform.FindChild ("Muzzle");
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
        col.enabled = false;
    }
	
	// Update is called once per frame
	public virtual void Update () {
		if (control) {
			inputData = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			move (inputData);
			if (Input.GetAxisRaw ("Fire") > 0f && _lastFrameAxis == 0f) {
				shoot (shotType);
				rofTimer = 0f;
			}
			if (Input.GetAxisRaw ("Fire") > 0f && _lastFrameAxis == 1f) {
				if (rofTimer > rateOfFire) {
					shoot (shotType);
					rofTimer = 0f;
				}
				rofTimer += Time.deltaTime;
			}
			_lastFrameAxis = Input.GetAxisRaw ("Fire");
		}

		if (invulnTime > 0f) {
			invulnTime -= Time.deltaTime;
		} else {
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
			col.enabled = true;
		}
	}

	private void move(Vector2 movement) {
		rigidBody.velocity = movement.normalized * velocity;
	}

	private void shoot(int type = 1) {
		if (type == 1) {
			Instantiate (bulletPrefab, muzzle.position, Quaternion.identity);
		} else if (type == 2) {
			Instantiate (bulletPrefab, muzzle.position, Quaternion.Euler(0f,0f,-15f));
			Instantiate (bulletPrefab, muzzle.position, Quaternion.Euler(0f,0f,15f));
		} else if (type == 3) {
			Instantiate (bulletPrefab, muzzle.position, Quaternion.identity);
			Instantiate (bulletPrefab, muzzle.position, Quaternion.Euler(0f,0f,-15f));
			Instantiate (bulletPrefab, muzzle.position, Quaternion.Euler(0f,0f,15f));
		}
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

	public void toggleControls () {
		if (control)
			control = false;
		else
			control = true;
	}

	public void destroyPlayer () {
        if(OnPlayerDied != null)
        {
            OnPlayerDied();
        }
		Destroy (gameObject);
	}

//	public void resetPlayer () {
//		
//		shotType = 1;
//	}
}