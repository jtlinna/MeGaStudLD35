using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

    [SerializeField]
    private float ScrollSpeed = 5f;

    private float _scrollSpeed;

	private Transform[] backGrounds;

    void Awake()
    {
		backGrounds = new Transform[2];
		backGrounds[0] = transform.GetChild(0).transform;
		backGrounds[1] = transform.GetChild(1).transform;
    }

    void Update()
    {
		foreach(Transform pic in backGrounds){
			pic.localPosition = new Vector2(pic.localPosition.x, pic.localPosition.y + (ScrollSpeed * Time.deltaTime));
			if (pic.localPosition.y >= 64f)
				pic.localPosition = new Vector2(pic.localPosition.x, pic.localPosition.y - 128f);
			else if (pic.localPosition.y <= -64f)
				pic.localPosition = new Vector2(pic.localPosition.x, pic.localPosition.y + 128f);
		}
    }
}
