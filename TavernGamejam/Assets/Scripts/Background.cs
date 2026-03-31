using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(110)]
public class Background : MonoBehaviour
{
    [Serializable]
    public class ParallexBackground
    {
        public Sprite sprite;
        public Vector2 offset;
        public float parallaxFactor;

        Transform transform => _gameObject.transform;
        GameObject _gameObject;
        SpriteRenderer _renderer;

        public void Init(GameObject gameObject)
        {
            _gameObject = gameObject;
            _renderer = _gameObject.GetComponent<SpriteRenderer>();
            _renderer.drawMode = SpriteDrawMode.Tiled;
            _renderer.size = new Vector2(500, _renderer.sprite.rect.size.y / _renderer.sprite.pixelsPerUnit);
            _renderer.sprite = sprite;
        }

        public void Update(Vector2 cameraPosition, float z)
        {
            Vector2 p = offset + cameraPosition * parallaxFactor;
            transform.position = new Vector3(p.x, p.y, z);
        }
    }

    public ParallexBackground[] backgrounds;
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
        SetBackgroundObjects();
        for (int i = 0;i < backgrounds.Length; i++)
        {
            backgrounds[i].Init(transform.GetChild(i).gameObject);
        }
    }
    void LateUpdate()
    {
        for (int i = 0;i < backgrounds.Length; i++)
        {
            backgrounds[i].Update(Camera.main.transform.position, transform.position.z + i*0.01f);
        }
    }

    GameObject CreateBackgroundObject()
    {
        GameObject obj = new GameObject("Background");
        obj.transform.parent = transform;
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        return obj;
    }

    void SetBackgroundObjects()
    {
        int childCount = transform.childCount;
        for (int i = childCount; i < backgrounds.Length; i++) CreateBackgroundObject();
        for(int i = 0;i<backgrounds.Length; i++)
        {
            var background = backgrounds[i];
            var child = transform.GetChild(i);
            if (!child.TryGetComponent<SpriteRenderer>(out SpriteRenderer _))
                child.AddComponent<SpriteRenderer>();
        }
    }

    void OnValidate()
    {
        SetBackgroundObjects();
        for (int i = 0;i < backgrounds.Length; i++)
        {
            backgrounds[i].Init(transform.GetChild(i).gameObject);
            backgrounds[i].Update(Camera.main.transform.position, transform.position.z + i*0.01f);
        }
    }
}
