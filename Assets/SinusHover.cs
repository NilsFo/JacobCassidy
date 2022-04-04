using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SinusHover : MonoBehaviour
{
    public float speed = 0.69f;
    public float magnitude = 0.25f;
    private float dt;

    // Start is called before the first frame update
    void Start()
    {
        //Random jitter
        dt = Random.Range(0.0f, 200.0f);
    }

    // Update is called once per frame
    void Update()
    {
        dt += Time.deltaTime * speed;
        Vector2 pos = new Vector2(0, MathF.Sin(dt) * magnitude);
        transform.localPosition = pos;
    }
}