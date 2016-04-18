using UnityEngine;
using System.Collections;

public class BulletSequenceManager : MonoBehaviour {

	public enum bulletIdentifier
	{
		defaultBullet = 0,
		triangleBullet = 1,
		squareBullet = 2,
		pentaBullet = 3,
		hexBullet = 4,
		septiBullet = 5,
		octaBullet = 6
	}

	public GameObject[] bulletPrefabs;

	public IEnumerator spawnBullets(Transform[] spawns, BulletType bulletID = BulletType.DEFAULT, int burstAmount = 1, float timeBetweenShots = 0f, float timeBetweenBursts = 0f, float timeBetweenCycles = 0f){
		
		while (true) {
			for (int i = 0; i < burstAmount; i++) {
				if (bulletID != BulletType.PENTAGON) {
					foreach (Transform spawn in spawns) {
						if (spawn.gameObject.activeSelf){
							BulletPoolManager.Instance.Spawn (bulletID, spawn.position, spawn.rotation);
							if (timeBetweenShots > 0f)
								yield return new WaitForSeconds (timeBetweenShots);
						}
					}
				} else {
					for (int j = 0; j < spawns.Length; j++) {
                        GameObject go = GameObject.FindGameObjectWithTag("Player");
                        Vector3 vectorToPlayer = Vector2.down;
                        if (go != null)
                        {
                            vectorToPlayer = go.transform.position - spawns[j].transform.position;
                        }
                        if (spawns[j].gameObject.activeSelf) BulletPoolManager.Instance.Spawn(bulletID, spawns[j].position, Quaternion.AngleAxis(-90f + ((-14f + (7f * j)) + (Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg)), Vector3.forward));
					}
				}

				if (burstAmount > 1 && timeBetweenBursts > 0f)
					yield return new WaitForSeconds (timeBetweenBursts);
			}

			yield return new WaitForSeconds (timeBetweenCycles);
		}
	}

	public void stopSpawning () {
		StopAllCoroutines ();
	}
}
