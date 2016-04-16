using UnityEngine;
using System.Collections;

public class EnemyHealth : BaseHealth {

    [SerializeField]
    private BaseEnemy EnemyAI;

    protected override void Awake()
    {
        base.Awake();
        if(EnemyAI == null)
        {
            EnemyAI = GetComponent<BaseEnemy>();
        }
    }

    public override void TakeDamage(float damage)
    {
        EnemyAI.ChangeShape();
    }

}
