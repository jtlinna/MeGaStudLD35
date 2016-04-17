﻿using UnityEngine;
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

	public IEnumerator spawnBullets(Transform[] spawns, bulletIdentifier bulletID = bulletIdentifier.defaultBullet, int burstAmount = 1, float timeBetweenShots = 0f, float timeBetweenBursts = 0f, float timeBetweenCycles = 0f){
		shotDone = false;

		while (!shotDone) {
			if (shotDone)
				break;
			for (int i = 0; i < burstAmount; i++) {
				if (shotDone)
					break;
				if (bulletID != bulletIdentifier.pentaBullet) {
					foreach (Transform spawn in spawns) {
						if (shotDone)
							break;

						if (spawn.gameObject.activeSelf) Instantiate (bulletPrefabs [(int)bulletID], spawn.position, spawn.rotation);
					
						if (timeBetweenShots > 0f)
							yield return new WaitForSeconds (timeBetweenShots);
					}
				} else {
					for (int j = 0; j < spawns.Length; j++) {
						if (shotDone)
							break;
						Vector3 vectorToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - spawns[j].transform.position;
						if (spawns[j].gameObject.activeSelf) Instantiate (bulletPrefabs [(int)bulletID], spawns[j].position, Quaternion.AngleAxis(-90f + ((-14f + (7f * j)) + (Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg)), Vector3.forward));
					}
				}
				if (shotDone)
					break;
				
				if (burstAmount > 1 && timeBetweenBursts > 0f)
					yield return new WaitForSeconds (timeBetweenBursts);
			}
			if (shotDone)
				break;
			yield return new WaitForSeconds (timeBetweenCycles);
		}

		shotDone = true;
	}
}