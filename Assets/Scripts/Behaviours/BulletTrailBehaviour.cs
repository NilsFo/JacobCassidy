using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailBehaviour : MonoBehaviour {

    public Vector3 startPos;
    public Vector3 hitPos;
    
    private LineRenderer lineRenderer;
    public Color color = Color.white;

    public float lifeTime = 1.5f;
    public float speed = 10f;
    private float _life = 0f;
    private float _length = 0f;

    public float extraLength = 0.5f;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0,startPos);
        lineRenderer.SetPosition(1,startPos);
        _length = (startPos - hitPos).magnitude + extraLength;
    }
    
    // Update is called once per frame
    void Update()
    {
        _life += Time.deltaTime;
        if (_life * speed < _length) {
            lineRenderer.SetPosition(1, startPos + (hitPos-startPos).normalized * (_life * speed));
        }
        var alpha = 1 - (_life / lifeTime);
        alpha *= alpha;
        color.a = alpha;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        if (_life > lifeTime) {
            Destroy(gameObject);
        }
    }
}
