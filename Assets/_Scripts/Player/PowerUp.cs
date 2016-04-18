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

    void Awake()
    {
        Add(this);
    }

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

    void OnDestroy()
    {
        Remove(this);
    }

	// Update is called once per frame
	void Update () {
		transform.position += -transform.up.normalized * MoveSpeed * Time.deltaTime;
	}

	public virtual void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Player") || other.CompareTag("BottomEdge")) {
			Destroy (gameObject);
		}
	}
}
