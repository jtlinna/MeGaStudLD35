using UnityEngine;
using System.Collections.Generic;

public enum PowerUpIdentifier
{
    NONE = -1,
    LIFE = 1,
    SCORE_MULTIPLIER = 2,
    WEAPON = 3,
    FIRERATE = 4,
    BOMB = 5
}

public class PowerUp : MonoBehaviour {

    private static List<PowerUp> ActivePowerUps = new List<PowerUp>();

    public static void Add(PowerUp pu)
    {
        if(!ActivePowerUps.Contains(pu))
        {
            ActivePowerUps.Add(pu);
        }
    }

    public static void Remove(PowerUp pu)
    {
        if(ActivePowerUps.Contains(pu))
        {
            ActivePowerUps.Remove(pu);
        }
    }

    public static List<PowerUp> GetActivePowerUps()
    {
        return ActivePowerUps;
    }

	protected GameManager manager;
	[SerializeField] private float MoveSpeed = 3f;

    private Transform _player;
    void Awake()
    {
        Add(this);
    }

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            _player = go.transform;
        }
	}

    void OnDestroy()
    {
        Remove(this);
    }

	// Update is called once per frame
	void Update () {
		transform.position += -transform.up.normalized * MoveSpeed * Time.deltaTime;
        Vector3 offset = Vector3.zero;

        if (_player != null)
        {
            if (_player.position.x <= transform.position.x)
                offset = -transform.right;
            else
                offset = transform.right;
        }
        transform.position += offset * Time.deltaTime * 0.5f;
	}

	public virtual void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Player") || other.CompareTag("BottomEdge") || other.CompareTag("Edge")) {
			Destroy (gameObject);
		}
	}
}
