using UnityEngine;
using System.Collections;

public class BaseHealth : MonoBehaviour {

    [SerializeField]
    private float MaxHealth;

    private float _currentHealth;

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

    }

}
