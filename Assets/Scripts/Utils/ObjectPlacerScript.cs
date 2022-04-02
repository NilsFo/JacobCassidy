using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPlacerScript : MonoBehaviour {
    public float zRange = 10f;
    public float pixelsPerUnit = 16;

    public void AlignObjects() {
        foreach (var t in GetComponentsInChildren<Transform>()) {
            t.localPosition = new Vector3(Mathf.Round(t.localPosition.x * pixelsPerUnit) / pixelsPerUnit,
                Mathf.Round(t.localPosition.y * pixelsPerUnit) / pixelsPerUnit,
                t.localPosition.z
            );
        }
    }

    public void SetZCoord() {
        var tilemaps = FindObjectsOfType<Tilemap>();
        float yMin = float.MaxValue, yMax = float.MinValue;
        if (tilemaps == null || tilemaps.Length == 0)
        {
            Debug.LogWarning("Warning: No tilemap assigned to this fixer on " + gameObject.name, gameObject);
            return;
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
        
        // Set only for immediate children
        foreach (var t in transform.Cast<Transform>().SelectMany(t => t.GetComponents<Transform>())) {
            float z = ((t.position.y - yMin) / (yMax - yMin) * zRange) - zRange;
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, z);
            var further = t.GetComponent<ObjectPlacerScript>();
            if(further != null)
                further.SetZCoord();
        }
        this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
}
