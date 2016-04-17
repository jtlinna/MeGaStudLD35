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
    private int _prevWaypoint;

    private float _moveTimer;
    private float _arcMoveTime;
    private float _moveSpeed;

    private bool _allowPattern = true;

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
        if (!_allowPattern)
            return;

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
                Move_Infinity();
				break;
        }
    }

	public Transform getWaypoint(int index, bool reverse = false) {
		return reverse ? rWaypoints[index] : waypoints[index];
	}

    private void NextWaypoint()
    {
        _prevWaypoint = _currentWaypoint;
        _currentWaypoint++;
        if (_currentWaypoint >= waypoints.Length)
            _currentWaypoint = 0;

        Transform newWaypoint = getWaypoint(_currentWaypoint);
        if (!newWaypoint.gameObject.activeInHierarchy)
            _currentWaypoint = 0;

        _moveTimer = 0f;
    }

	public Transform getNearestWaypoint (out int waypointIndex) {
		float distance = Mathf.Infinity;
		Transform nearestTrans = null;
        waypointIndex = 0;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform trans = waypoints[i];
            if (!trans.gameObject.activeInHierarchy)
                continue;

			float dist = Vector2.Distance(MoveObject.transform.position, trans.position);
			if (dist < distance) {
                waypointIndex = i;
				distance = dist;
				nearestTrans = trans;
			}
		}
        NextWaypoint();
		return nearestTrans;
	}

    public void MoveToNearestWaypoint()
    {
        Transform nearestWaypoint = getNearestWaypoint(out _currentWaypoint);
        NextWaypoint();
        StartCoroutine(MoveToNearestRoutine(nearestWaypoint));
    }

	public void reset (Shapes newShape) {
		foreach (Transform trans in waypoints) {
			trans.gameObject.SetActive (true);
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.Euler(Vector3.zero);
		}

        _currentWaypoint = 0;
        _prevWaypoint = -1;
        
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
			trans.localPosition = trans.up * scale;

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

        transform.position = new Vector3(transform.position.x, waypoints[0].position.y);

        foreach (Transform trans in waypoints)
        {
            trans.localPosition += trans.up * scale;
            trans.localPosition = new Vector3(trans.localPosition.x, 0, trans.localPosition.z);
        }
        
        _currentShape = Shapes.curve;
        float arcLength = 90f * (Mathf.PI / 180) * scale;
        _arcMoveTime = arcLength / 10f;
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
			if (i < 4)
				waypoints [i].gameObject.SetActive (true);
			else
				waypoints [i].gameObject.SetActive (false);
		}

		waypoints [0].localPosition = new Vector2 (0f, 0f) * scale;
		waypoints [1].localPosition = new Vector2 (1f, 0f) * scale;
		waypoints [2].localPosition = new Vector2 (0f, 0f) * scale;
        waypoints[3].localPosition = new Vector2(-1f, 0f) * scale;
        //waypoints [4].localPosition = new Vector2 (-1f, 0f) * scale;
        //waypoints [5].localPosition = new Vector2 (-1f, 0f) * scale;

        _currentShape = Shapes.infinity;
    }

    private void Move_Curve()
    {
        _moveTimer += Time.deltaTime;

        Vector3 center = transform.position + (Vector3.up * shapeScale);
        Vector3 leftRelCenter = getWaypoint(0).position - center;
        Vector3 rightRelCenter = getWaypoint(1).position - center;

        if (_moveTimer > _arcMoveTime)
            _moveTimer = _arcMoveTime;
        if (_currentWaypoint == 0)
        {
            MoveObject.transform.position = Vector3.Slerp(leftRelCenter, rightRelCenter, _moveTimer / _arcMoveTime);
            MoveObject.transform.position += center;
            if (MoveObject.transform.position == getWaypoint(1).position)
            {
                NextWaypoint();
            }
        }
        else if(_currentWaypoint == 1)
        {
            MoveObject.transform.position = Vector3.Slerp(rightRelCenter, leftRelCenter, _moveTimer / _arcMoveTime);
            MoveObject.transform.position += center;

            if (MoveObject.transform.position == getWaypoint(0).position)
            {
                NextWaypoint();
            }
        }
    }

    private void Move_Envelope()
    {
        float speed = 10f;
        MoveObject.transform.position = Vector2.MoveTowards(MoveObject.transform.position, getWaypoint(_currentWaypoint).position, speed * Time.deltaTime);
        if (MoveObject.transform.position == getWaypoint(_currentWaypoint).position)
            NextWaypoint();
    }

    private void Move_Hourglass()
    {
        float speed = 10f;
        MoveObject.transform.position = Vector2.MoveTowards(MoveObject.transform.position, getWaypoint(_currentWaypoint).position, speed * Time.deltaTime);
        if (MoveObject.transform.position == getWaypoint(_currentWaypoint).position)
            NextWaypoint();
    }

    private void Move_Star()
    {
        float speed = 10f;
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

    private void Move_Infinity()
    {
        if (_prevWaypoint == -1)
            NextWaypoint();
        
        Vector3 center = Vector3.zero;
        if(_currentWaypoint == 1 && _prevWaypoint == 0)
        {
            center = transform.position + Vector3.down + Vector3.right * 0.5f * shapeScale;
        }
        else if(_prevWaypoint == 1 && _currentWaypoint == 2)
        {
            center = transform.position + Vector3.up + Vector3.right * 0.5f * shapeScale;
        }
        else if(_prevWaypoint == 2 && _currentWaypoint == 3)
        {
            center = transform.position + Vector3.down + Vector3.left * 0.5f * shapeScale;
        }
        else if(_prevWaypoint == 3 && _currentWaypoint == 0)
        {
            center = transform.position + Vector3.up + Vector3.left * 0.5f * shapeScale;
        }
        else
        {
            Debug.LogError("What's going on with Infinity Waypoint?!");
        }

        _moveTimer += Time.deltaTime;
        if(_moveTimer > 2f)
        {
            _moveTimer = 2f;
        }

        Vector3 leftRelCenter = getWaypoint(_prevWaypoint).position - center;
        Vector3 rightRelCenter = getWaypoint(_currentWaypoint).position - center;
        MoveObject.transform.position = Vector3.Slerp(leftRelCenter, rightRelCenter, _moveTimer / 2f);
        MoveObject.transform.position += center;
        if(MoveObject.transform.position == getWaypoint(_currentWaypoint).position)
        {
            NextWaypoint();
        }
    }

    private IEnumerator MoveToNearestRoutine(Transform trans)
    {
        _allowPattern = false;
        while(MoveObject.transform.position != trans.position)
        {
            MoveObject.transform.position = Vector3.MoveTowards(MoveObject.transform.position, trans.position, 10f * Time.deltaTime);
            yield return null;
        }
        _allowPattern = true;
        Debug.Log("Current WP: " + _currentWaypoint);
    }
}
