using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class ScrollingBackground : MonoBehaviour {

    [SerializeField]
    private float ScrollSpeed = 0.25f;

    private float _scrollSpeed;

    private Material _backgroundMaterial;

    void Awake()
    {
        _backgroundMaterial = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if(_backgroundMaterial != null)
        {
            Vector2 offset = _backgroundMaterial.mainTextureOffset;
            offset.y += (ScrollSpeed / 100f * Time.deltaTime); // Just to get a reasonable scale for the speed
            _backgroundMaterial.mainTextureOffset = offset;
        }
    }
}
