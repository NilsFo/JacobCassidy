using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonewallBehaviourScript : MonoBehaviour
{
    [SerializeField] private float stonewallDuration = 5f;
    [SerializeField] private float stonewallTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        stonewallTimer = stonewallDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (stonewallTimer < Time.deltaTime)
        {
            Destroy(gameObject);
        }
        else
        {
            stonewallTimer -= Time.deltaTime;
        }
    }
}
