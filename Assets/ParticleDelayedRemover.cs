using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDelayedRemover : MonoBehaviour
{
    private bool removing = false;
    public float removerTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        removing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (removing)
        {
            removerTimer -= Time.deltaTime;
            if (removerTimer < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Remove(float i)
    {
        removerTimer = i;
        transform.parent = null;
        removing = true;
    }
}