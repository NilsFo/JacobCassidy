using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class StonewallBehaviourScript : MonoBehaviour
{
    [SerializeField] private float stonewallDuration = 5f;
    [SerializeField] private float stonewallTimer = 0f;
    
    private AstarPath path;
    private Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        stonewallTimer = stonewallDuration;
        
        path = FindObjectOfType<AstarPath>();
        bounds = GetComponent<Collider2D>().bounds;

        UpdateNavmesh();
    }


    // Update is called once per frame
    void Update()
    {
        

        if (stonewallTimer < Time.deltaTime)
        {
            Destroy(gameObject);
            UpdateNavmesh();
        }
        else
        {
            stonewallTimer -= Time.deltaTime;
        }
    }

    void UpdateNavmesh() {
        // Updating pathfinding
        GraphUpdateObject guo = new GraphUpdateObject(bounds);
        guo.updatePhysics = true;
        path.UpdateGraphs(guo);
    }
}
