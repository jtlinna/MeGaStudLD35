using UnityEngine;
using System.Collections;

public class BaseHealth : MonoBehaviour {

    [SerializeField]
    private float MaxHealth;

    private float _currentHealth;

    protected virtual void Awake()
    {
        _currentHealth = MaxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

}
