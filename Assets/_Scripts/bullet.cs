using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	[SerializeField] private float speed;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, speed);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(CompareTag("eBullet") && other.CompareTag("Player"))
        {
            DealDamage(other.gameObject);
        }
        else if(CompareTag("pBullet") && other.CompareTag("Enemy"))
        {
            DealDamage(other.gameObject);
        }
    }

    void DealDamage(GameObject other)
    {
        BaseHealth health = other.GetComponentInParent<BaseHealth>();
        if (health != null)
        {
            health.TakeDamage(25f);
        }

        Destroy(gameObject);
    }
}
