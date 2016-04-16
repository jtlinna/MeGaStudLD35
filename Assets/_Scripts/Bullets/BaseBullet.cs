using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour {

    [SerializeField]
    protected float Damage;
    [SerializeField]
    protected float MoveSpeed;
    [SerializeField]
    protected string TargetTag;

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
        //if(other.CompareTag("BulletBoundary"))
        //{
        //    Trigger(null);
        //}
    }
}
