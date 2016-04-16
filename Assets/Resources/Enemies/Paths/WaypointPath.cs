using UnityEngine;
using System.Collections;

public class WaypointPath : MonoBehaviour {

	public Transform[] waypoints, rWaypoints;
	private int length;
	public bool star = false, curve = false;
	public float shapeScale = 1f;

	void Awake () {
		length = transform.childCount;
		waypoints = new Transform[length];
		rWaypoints = new Transform[length];
		for (int i = 0; i < length; i++)
			waypoints [i] = transform.GetChild (i);
		for (int i = 0; i < length; i++) {
			rWaypoints [i] = waypoints [(length - 1) - i];
		}

		if (star)
			setStar (shapeScale);
		if (curve)
			setCurve (shapeScale);
	}

	public Transform getWaypoint(int index, bool reverse = false) {
		return reverse ? rWaypoints[index] : waypoints[index];
	}

	private void setStar (float scale = 1f) {
		waypoints [0].Rotate (0f,0f,0f);
		waypoints [1].Rotate (0f,0f,(360f / 5f) * 2f);
		waypoints [2].Rotate (0f,0f,(360f / 5f) * 4f);
		waypoints [3].Rotate (0f,0f,(360f / 5f) * 1f);
		waypoints [4].Rotate (0f,0f,(360f / 5f) * 3f);

		foreach (Transform trans in waypoints)
			trans.localPosition += trans.up * scale;
	}

	private void setCurve (float scale = 1f) {
		waypoints [0].Rotate (0f, 0f, 135f);
		waypoints [1].Rotate (0f, 0f, 225f);

		foreach (Transform trans in waypoints)
			trans.localPosition += trans.up * scale;
	}
}
