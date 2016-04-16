using UnityEngine;
using System.Collections;

public class BasicBullet : BaseBullet {

    protected override void DoMovement()
    {
        base.DoMovement();
        transform.position += MoveSpeed * transform.up * Time.deltaTime;
    } 
}
