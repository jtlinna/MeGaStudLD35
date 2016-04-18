using UnityEngine;
using System.Collections;

public class DestroyOnTrigger : MonoBehaviour {

    [SerializeField]
    private string tag;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tag))
            Destroy(GetComponentInParent<BaseEnemy>().gameObject);
    }
}