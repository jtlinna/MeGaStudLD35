using UnityEngine;
using System.Collections;

public class BulletSequenceManager : MonoBehaviour {


	public enum bulletIdentifier
	{
		defaultBullet = 0
	}

	public GameObject[] bulletPrefabs;

	public IEnumerator spawnBullets(Transform[] spawns, bulletIdentifier bulletID = bulletIdentifier.defaultBullet, int burstAmount = 1, float timeBetweenShots = 0f, float timeBetweenBursts = 0f){
		for (int i = 0; i < burstAmount; i++) {
			foreach (Transform spawn in spawns) {
				Instantiate (bulletPrefabs [(int)bulletID], spawn.position, spawn.rotation);
				
				if (timeBetweenShots > 0f)
					yield return new WaitForSeconds (timeBetweenShots);
			}
			if (burstAmount > 1 && timeBetweenBursts > 0f)
				yield return new WaitForSeconds (timeBetweenBursts);
		}
	}
}
