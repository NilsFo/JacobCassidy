using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisibilityTrigger : MonoBehaviour
{ 
    private SpriteRenderer mySprite;
    public bool showSprite = true;
    public float fadeSpeed = 2f;
    private float _fade;
    private Color initialColor;
    
    // Start is called before the first frame update
    void Start() {
        mySprite = GetComponent<SpriteRenderer>();
        _fade = showSprite ? 1.0f : 0.0f;
        initialColor = mySprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (showSprite && _fade < 1f) {
            _fade += fadeSpeed * Time.deltaTime;
            initialColor.a = Mathf.Min(_fade, 1.0f);
            mySprite.color = initialColor;
        }
        else if (!showSprite && _fade > 0f) {
            _fade -= fadeSpeed * Time.deltaTime;
            initialColor.a = Mathf.Max(_fade, 0f);
            mySprite.color = initialColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerMovementBehaviour>()) {
            Debug.Log("Showing inside buildings");
            showSprite = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<PlayerMovementBehaviour>()) {
            Debug.Log("Hiding inside buildings");
            showSprite = true;
        }
    }

}
