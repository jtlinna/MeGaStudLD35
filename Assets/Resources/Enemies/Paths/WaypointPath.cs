using UnityEngine;
using System.Collections;

public class WaypointPath : MonoBehaviour {

	public enum shapes {star, curve, hourglass, envelope};
	public Transform[] waypoints, rWaypoints;
	private int length;
	public shapes shape;
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

		switch (shape) {
		case shapes.star:
			setStar (shapeScale);
			break;
		case shapes.curve:
			setCurve (shapeScale);
			break;
		case shapes.hourglass:
			setHourglass (shapeScale);
			break;
		case shapes.envelope:
			setEnvelope (shapeScale);
			break;
		}
	}

	public Transform getWaypoint(int index, bool reverse = false) {
		return reverse ? rWaypoints[index] : waypoints[index];
	}

	public void reset (int waypointAmount, shapes newShape) {
		foreach (Transform trans in waypoints) {
			trans.gameObject.SetActive (true);
			trans.localPosition = Vector3.zero;
			trans.localRotation = Quaternion.Euler(Vector3.zero);
		}

		switch (newShape) {
		case shapes.star:
			setStar (shapeScale);
			break;
		case shapes.curve:
			setCurve (shapeScale);
			break;
		case shapes.hourglass:
			setHourglass (shapeScale);
			break;
		case shapes.envelope:
			setEnvelope (shapeScale);
			break;
		}
			
	}

	private void setStar (float scale = 1f) {
		for (int i = 0; i < length; i++) {
			if (i < 5)
				waypoints [i].gameObject.SetActive (true);
			else
				waypoints [i].gameObject.SetActive (false);
		}

		waypoints [0].Rotate (0f,0f,0f);
		waypoints [1].Rotate (0f,0f,(360f / 5f) * 2f);
		waypoints [2].Rotate (0f,0f,(360f / 5f) * 4f);
		waypoints [3].Rotate (0f,0f,(360f / 5f) * 1f);
		waypoints [4].Rotate (0f,0f,(360f / 5f) * 3f);

		foreach (Transform trans in waypoints)
			trans.localPosition += trans.up * scale;
	}

	private void setCurve (float scale = 1f) {
		for (int i = 0; i < length; i++) {
			if (i < 2)
				waypoints [i].gameObject.SetActive (true);
			else
				waypoints [i].gameObject.SetActive (false);
		}

		waypoints [0].Rotate (0f, 0f, 135f);
		waypoints [1].Rotate (0f, 0f, 225f);

		foreach (Transform trans in waypoints)
			trans.localPosition += trans.up * scale;
	}

	private void setHourglass (float scale = 1f) {
		for (int i = 0; i < length; i++) {
			if (i < 4)
				waypoints [i].gameObject.SetActive (true);
			else
				waypoints [i].gameObject.SetActive (false);
		}

		waypoints [0].localPosition = new Vector2 (1f, 0.5f) * scale;
		waypoints [1].localPosition = new Vector2 (1f, -0.5f) * scale;
		waypoints [2].localPosition = new Vector2 (-1f, 0.5f) * scale;
		waypoints [3].localPosition = new Vector2 (-1f, -0.5f) * scale;
	}

	private void setEnvelope (float scale = 1f) {
		for (int i = 0; i < length; i++) {
			if (i < 5)
				waypoints [i].gameObject.SetActive (true);
			else
				waypoints [i].gameObject.SetActive (false);
		}

		waypoints [0].localPosition = new Vector2 (1f, 0f) * scale;
		waypoints [1].localPosition = new Vector2 (1f, 1f) * scale;
		waypoints [2].localPosition = new Vector2 (0f, -1f) * scale;
		waypoints [3].localPosition = new Vector2 (-1f, 1f) * scale;
		waypoints [4].localPosition = new Vector2 (-1f, -0f) * scale;
	}
}
