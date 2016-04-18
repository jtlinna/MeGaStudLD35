using UnityEngine;
using System.Collections.Generic;

public enum BulletType
{
    DEFAULT = 0,
    PLAYER = 1,
    TRIANGLE = 2,
    SQUARE = 3,
    PENTAGON = 4,
    HEXAGON = 5,
    SEPTIGON = 6,
    OCTAGON = 7,
    BOSS_S1 = 8,
    BOSS_S3 = 9,
    BOSS_S4 = 10,
    BOSS_S5 = 11
}

public class BaseBullet : MonoBehaviour {

    private static List<BaseBullet> ActiveBullets = new List<BaseBullet>();

	[SerializeField] private GameObject bulletHitPrefab;
    
    public static void AddBullet(BaseBullet bullet)
    {
        ActiveBullets.Add(bullet);
    }

    public static void RemoveBullet(BaseBullet bullet)
    {
        if (ActiveBullets.Contains(bullet))
            ActiveBullets.Remove(bullet);
    }

    public static void RemoveAllBullets(bool destroyObject = false)
    {
        if(destroyObject)
        {
            for (int i = ActiveBullets.Count - 1; i >= 0; i--)
            {
                ActiveBullets[i].gameObject.SetActive(false);
            }
        }

        ActiveBullets.Clear();
    }

    [SerializeField]
    protected float Damage;
    [SerializeField]
    protected float MoveSpeed;
    [SerializeField]
    protected string TargetTag;
	[SerializeField]
	protected Vector3 _lastPos;

    protected virtual void OnEnable()
    {
        AddBullet(this);
    }

    void OnDisable()
    {
        RemoveBullet(this);
    }

    void Update()
    {
        DoMovement();
    }

    protected virtual void DoMovement()
    {
		
    }
    
    protected void Trigger(GameObject target)
    {
        if (target != null)
        {
            BaseHealth health = target.GetComponentInParent<BaseHealth>();
            if (health != null)
            {
                health.TakeDamage(Damage);
            }
        }

        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(TargetTag))
        {
			SoundManager.Instance.PlayClip (SoundType.BULLET_HIT);
			Instantiate (bulletHitPrefab, transform.position, Quaternion.identity);
            Trigger(other.gameObject);
        }
        if(other.CompareTag("BottomEdge") || other.CompareTag("Edge"))
        {
            Trigger(null);
        }
    }
}
