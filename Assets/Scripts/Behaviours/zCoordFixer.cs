using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class zCoordFixer : MonoBehaviour
{
    // This script sets the z Coordinate linearly to the y Coordinate to fake occlusion

    public float zRange = 10f;
    public float yOffset = 0;

    public bool zFixingEnabled = true;

    private float yMin = float.MaxValue, yMax = float.MinValue;

    void Awake() {
        var tilemaps = FindObjectsOfType<Tilemap>();
        
        if (tilemaps == null || tilemaps.Length == 0)
        {
            Debug.LogWarning("Warning: No tilemap assigned to this fixer on " + gameObject.name, gameObject);
            zFixingEnabled = false;
        }
        else
        {
            foreach (var tilemap in tilemaps) {
                if (tilemap.isActiveAndEnabled) {
                    yMin = Mathf.Min(yMin, tilemap.layoutGrid.CellToWorld(tilemap.cellBounds.min).y);
                    yMax = Mathf.Max(yMax, tilemap.layoutGrid.CellToWorld(tilemap.cellBounds.max).y);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!zFixingEnabled)
        {
            return;
        }

        var t = transform;
        float z = ((t.position.y + yOffset - yMin) / (yMax - yMin) * zRange) - zRange;
        t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, z);
    }
}