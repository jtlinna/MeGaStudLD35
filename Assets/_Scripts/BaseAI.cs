using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour {

    public BaseHealth Health;

    [SerializeField]
    protected float MoveSpeed;

    public virtual void HandleMovement(Vector2 movement)
    {

    }
}
