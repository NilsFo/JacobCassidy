using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailBehaviour : MonoBehaviour {

    public Vector3 startPos;
    public BulletBehaviourScript bullet;
    private LineRenderer lineRenderer;
    public Color color = Color.white;

    public float lifeTime = 1.5f;
    private float _lifeLeft;
    private bool bulletStillAlive = true;
    
    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0,startPos);
        lineRenderer.SetPosition(1,startPos);
        _lifeLeft = lifeTime;
    }
    
    // Update is called once per frame
    void Update()
    {
        _lifeLeft -= Time.deltaTime;
        if (bulletStillAlive && bullet == null) {
            bulletStillAlive = false;
        }
        if(bulletStillAlive)
            lineRenderer.SetPosition(1, bullet.transform.position);
        var alpha = _lifeLeft / lifeTime;
        alpha *= 2;
        color.a = alpha;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        if (_lifeLeft < 0) {
            Destroy(gameObject);
        }
    }
}
