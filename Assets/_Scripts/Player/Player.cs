using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[SerializeField] private float velocity = 10f;
	private Rigidbody2D rigidBody;
	private Collider2D col;
	private Vector2 inputData;
	private Transform muzzle;

	public BaseHealth health;
	public GameObject bulletPrefab;
	public int shotType = 1;
	private float invulnTime = 3f;

	// Use this for initialization
	public virtual void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		col = GetComponent<Collider2D> ();
		muzzle = transform.FindChild ("Muzzle");
	}
	
	// Update is called once per frame
	public virtual void Update () {
		inputData = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		move (inputData);

		if (Input.GetKeyDown(KeyCode.Space))
			shoot (shotType);

		if (invulnTime > 0f) {
			invulnTime -= Time.deltaTime;
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.4f);
			col.enabled = false;
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
			return true;
		}
		else
			return false;
	}

//	public void resetPlayer () {
//		
//		shotType = 1;
//	}
}