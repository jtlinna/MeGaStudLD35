using UnityEngine;
using System.Collections;

public class WaypointPath : MonoBehaviour {

	public enum Shapes {
        none = 0,
        star = 1,
        curve = 2,
        hourglass = 3,
        envelope = 4,
		infinity = 5 };

    public GameObject MoveObject;

	public Transform[] waypoints, rWaypoints;
	private int length;
	public Shapes shape;
	public float shapeScale = 1f;

    private Shapes _currentShape;
    private int _currentWaypoint;

    private float _moveTimer;

	void Awake () {
		length = transform.childCount;
		waypoints = new Transform[length];
		rWaypoints = new Transform[length];
		for (int i = 0; i < length; i++)
			waypoints [i] = transform.GetChild (i);
		for (int i = 0; i < length; i++) {
			rWaypoints [i] = waypoints [(length - 1) - i];
		}

		//switch (shape) {
		//case Shapes.star:
		//	setStar (shapeScale);
		//	break;
		//case Shapes.curve:
		//	setCurve (shapeScale);
		//	break;
		//case Shapes.hourglass:
		//	setHourglass (shapeScale);
		//	break;
		//case Shapes.envelope:
		//	setEnvelope (shapeScale);
		//	break;
		//}
	}

    void Update()
    {
        switch(_currentShape)
        {
            case Shapes.curve:
                Move_Curve();
                break;
            case Shapes.envelope:
                Move_Envelope();
                break;
            case Shapes.hourglass:
                Move_Hourglass();
                break;
            case Shapes.star:
                Move_Star();
                break;
			case Shapes.infinity:
				break;
        }
    }

	public Transform getWaypoint(int index, bool reverse = false) {
		return reverse ? rWaypoints[index] : waypoints[index];
	}

    private void NextWaypoint()
    {
        _currentWaypoint++;
        if (_currentWaypoint >= waypoints.Length)
            _currentWaypoint = 0;

        Transform newWaypoint = getWaypoint(_currentWaypoint);
        if (!newWaypoint.gameObject.activeInHierarchy)
            _currentWaypoint = 0;

        _moveTimer = 0f;
    }

	public Transform getNearestWaypoint () {
		float distance = Mathf.Infinity;
		Transform nearestTrans = null;
		foreach (Transform trans in waypoints) {
			float dist = Vector2.Distance(MoveObject.transform.position, trans.position);
			if (dist < distance) {
				distance = dist;
				nearestTrans = trans;
			}
		}
		return nearestTrans;
	}

	public void reset (Shapes newShape) {
		foreach (Transform trans in waypoints) {
			trans.gameObject.SetActive (true);
            trans.position = new Vector3(0, MoveObject.transform.position.y);/*Vector3.zero;*/
			trans.localRotation = Quaternion.Euler(Vector3.zero);
		}

		switch (newShape) {
		case Shapes.star:
			setStar (shapeScale);
			break;
		case Shapes.curve:
			setCurve (shapeScale);
			break;
		case Shapes.hourglass:
			setHourglass (shapeScale);
			break;
		case Shapes.envelope:
			setEnvelope (shapeScale);
			break;
		case Shapes.infinity:
			setInfinity (shapeScale);
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

        _currentShape = Shapes.star;
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

        _currentShape = Shapes.curve;
	}

	private void setHourglass (float scale = 1f) {
		for (int i = 0; i < length; i++) {
			if (i < 4)
				waypoints [i].gameObject.SetActive (true);
			else
				waypoints [i].gameObject.SetActive (false);
		}

		waypoints [0].localPosition = new Vector2 (1f, -0.5f) * scale;
		waypoints [1].localPosition = new Vector2 (1f, 0.5f) * scale;
		waypoints [2].localPosition = new Vector2 (-1f, -0.5f) * scale;
		waypoints [3].localPosition = new Vector2 (-1f, 0.5f) * scale;

        _currentShape = Shapes.hourglass;
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

        _currentShape = Shapes.envelope;
	}

	private void setInfinity (float scale = 1f) {
		for (int i = 0; i < length; i++) {
			if (i < 6)
				waypoints [i].gameObject.SetActive (true);
			else
				waypoints [i].gameObject.SetActive (false);
		}

		waypoints [0].localPosition = new Vector2 (0f, 0f) * scale;
		waypoints [1].localPosition = new Vector2 (1f, 0f) * scale;
		waypoints [2].localPosition = new Vector2 (1f, 0f) * scale;
		waypoints [3].localPosition = new Vector2 (0f, 0f) * scale;
		waypoints [4].localPosition = new Vector2 (-1f, 0f) * scale;
		waypoints [5].localPosition = new Vector2 (-1f, 0f) * scale;
	}

    private void Move_Curve()
    {
        _moveTimer += Time.deltaTime;
        if (_moveTimer > 2)
            _moveTimer = 2f;
        if (_currentWaypoint == 0)
        {

            MoveObject.transform.position = Vector3.Slerp(getWaypoint(0).position, getWaypoint(1).position, _moveTimer / 2f);
            
            if(MoveObject.transform.position == getWaypoint(1).position)
            {
                NextWaypoint();
            }
        }
        else
        {
            MoveObject.transform.position = Vector3.Slerp(getWaypoint(1).position, getWaypoint(0).position, _moveTimer / 2f);

            if (MoveObject.transform.position == getWaypoint(0).position)
            {
                NextWaypoint();
            }
        }
    }

    private void Move_Envelope()
    {
        float speed = (_currentWaypoint == 2 || _currentWaypoint == 3) ? 20f : 4f;
        MoveObject.transform.position = Vector2.MoveTowards(MoveObject.transform.position, getWaypoint(_currentWaypoint).position, speed * Time.deltaTime);
        if (MoveObject.transform.position == getWaypoint(_currentWaypoint).position)
            NextWaypoint();
    }

    private void Move_Hourglass()
    {
        float speed = (_currentWaypoint == 2 || _currentWaypoint == 0) ? 20f : 4f;
        MoveObject.transform.position = Vector2.MoveTowards(MoveObject.transform.position, getWaypoint(_currentWaypoint).position, speed * Time.deltaTime);
        if (MoveObject.transform.position == getWaypoint(_currentWaypoint).position)
            NextWaypoint();
    }

    private void Move_Star()
    {
        float speed = 20f;
        MoveObject.transform.position = Vector2.MoveTowards(MoveObject.transform.position, getWaypoint(_currentWaypoint).position, speed * Time.deltaTime);
        if (MoveObject.transform.position == getWaypoint(_currentWaypoint).position)
        {
            _moveTimer += Time.deltaTime;
            if (_moveTimer >= 0.5f)
            {
                _moveTimer = 0f;
                NextWaypoint();
            }
        }
    }
}
