using UnityEngine;
using System.Collections;

public class BulletSequenceManager : MonoBehaviour {

	public bool shotDone = true;

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

	public IEnumerator spawnBullets(Transform[] spawns, bulletIdentifier bulletID = bulletIdentifier.defaultBullet, int burstAmount = 1, float timeBetweenShots = 0f, float timeBetweenBursts = 0f){
		shotDone = false;
		for (int i = 0; i < burstAmount; i++) {
			if (shotDone)
				break;
			foreach (Transform spawn in spawns) {
				if (shotDone)
					break;

				Instantiate (bulletPrefabs [(int)bulletID], spawn.position, spawn.rotation);
				
				if (timeBetweenShots > 0f)
					yield return new WaitForSeconds (timeBetweenShots);
			}
			if (burstAmount > 1 && timeBetweenBursts > 0f)
				yield return new WaitForSeconds (timeBetweenBursts);

			if (shotDone)
				break;
		}
		shotDone = true;
	}
}
