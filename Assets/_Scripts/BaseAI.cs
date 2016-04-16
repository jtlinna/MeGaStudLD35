using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BaseHealth))]
public class BaseAI : MonoBehaviour {

    public BaseHealth Health;

    [SerializeField]
    protected float MoveSpeed;

    public virtual void HandleMovement(Vector3 movement)
    {

    }
}
