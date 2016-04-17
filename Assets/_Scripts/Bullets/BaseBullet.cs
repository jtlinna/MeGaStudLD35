using UnityEngine;
using System.Collections.Generic;

public class BaseBullet : MonoBehaviour {

    private static List<BaseBullet> ActiveBullets = new List<BaseBullet>();
    
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
                Destroy(ActiveBullets[i].gameObject);
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

    protected virtual void Awake()
    {
        AddBullet(this);
    }

    void OnDestroy()
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

        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(TargetTag))
        {
            Trigger(other.gameObject);
        }
        if(other.CompareTag("BottomEdge"))
        {
            Trigger(null);
        }
    }
}
