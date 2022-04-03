using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpriteBehaviour : MonoBehaviour {
    public SpriteRenderer gunS, gunSW, gunSE, gunW, gunE, gunNW, gunNE;
    public PlayerMovementBehaviour movementBehaviour;
    private SpriteRenderer _activeSprite;
    
    // Start is called before the first frame update
    void Start() {
        gunS.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        bool gunVisible = !(movementBehaviour.velocity.magnitude > 0.01f);
        if (!gunVisible) {
            SetSprite(null);
            return;
        }
        
        var direction = movementBehaviour.lookDirection;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < -150 || angle > 150) {
            SetSprite(gunW);
        }
        else if (angle > 120) {
            SetSprite(gunNW);
        }
        else if (angle < -120) {
            SetSprite(gunSW);
        }
        else if (angle > 60) {
            SetSprite(null);
        }
        else if (angle < -60) {
            SetSprite(gunS);
        }
        else if (angle > 30) {
            SetSprite(gunNE);
        }
        else if (angle < -30) {
            SetSprite(gunSE);
        }
        else {
            SetSprite(gunE);
        }
    }
    
    private void SetSprite(SpriteRenderer spriteRenderer) {
        if (_activeSprite == spriteRenderer) {
            return;
        }
        gunS.enabled = false;
        gunSW.enabled = false;
        gunSE.enabled = false;
        gunE.enabled = false;
        gunW.enabled = false;
        gunNW.enabled = false;
        gunNE.enabled = false;
        if (spriteRenderer != null) {
            spriteRenderer.enabled = true;
        }
        _activeSprite = spriteRenderer;
    }
}
