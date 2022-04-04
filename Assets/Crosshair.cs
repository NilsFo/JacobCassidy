using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    public FireGunBehaviourScript gun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        gun.
        transform.localPosition = (gun.aimDummy.transform.localPosition) * 7f;
    }
}
