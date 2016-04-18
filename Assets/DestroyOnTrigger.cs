using UnityEngine;
using System.Collections;

public class DestroyOnTrigger : MonoBehaviour {

    [SerializeField]
    private string tag;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tag))
        {

            Debug.Log("On trigger enter " + other.gameObject.name);
            if (tag != "Player")
            {
                BaseEnemy enemy = GetComponentInParent<BaseEnemy>();
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
                return;
            }

            Player player = other.GetComponentInParent<Player>();
            if(player != null)
            {
                ((PlayerHealth)player.health).Kill();
            }
        }
    }
}