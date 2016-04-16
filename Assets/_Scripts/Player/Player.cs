using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[SerializeField] private float velocity = 10f;
	private Rigidbody2D rigidBody;
	private Vector2 inputData;
	private Transform muzzle;

	public BaseHealth health;
	public GameObject bulletPrefab;
	public int shotType = 1;

	// Use this for initialization
	public virtual void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		muzzle = transform.FindChild ("Muzzle");
	}
	
	// Update is called once per frame
	public virtual void Update () {
		inputData = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		move (inputData);

		if (Input.GetKeyDown(KeyCode.Space))
			shoot (shotType);
	}

	private void move(Vector2 movement) {
		rigidBody.velocity = movement.normalized * velocity;
	}

	private void shoot(int type = 1) {
		GameObject temp;
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
}