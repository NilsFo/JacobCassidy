using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingVisibilityTrigger : MonoBehaviour {
    public Tilemap roofTilemap;
    public bool showBuildings = true;
    public float fadeSpeed = 0.1f;
    private float _fade;
    
    // Start is called before the first frame update
    void Start() {
        _fade = showBuildings ? 1.0f : 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (showBuildings && _fade < 1f) {
            _fade += fadeSpeed * Time.deltaTime;
            roofTilemap.color = new Color(1, 1, 1, Mathf.Min(_fade, 1.0f));
        }
        else if (!showBuildings && _fade > 0f) {
            _fade -= fadeSpeed * Time.deltaTime;
            roofTilemap.color = new Color(1, 1, 1, Mathf.Max(_fade, 0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerMovementBehaviour>()) {
            Debug.Log("Showing inside buildings");
            showBuildings = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<PlayerMovementBehaviour>()) {
            Debug.Log("Hiding inside buildings");
            showBuildings = true;
        }
    }


}
