using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[SerializeField] private float velocity = 10f;
	private Rigidbody2D rigidBody;
	private Vector2 inputData;
	private Transform muzzle;

	public BaseHealth health;
	public GameObject bulletPrefab;

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
			shoot ();
	}

	private void move(Vector2 movement) {
		rigidBody.velocity = movement.normalized * velocity;
	}

	private void shoot() {
		Instantiate (bulletPrefab, muzzle.position, Quaternion.identity);
	}
}