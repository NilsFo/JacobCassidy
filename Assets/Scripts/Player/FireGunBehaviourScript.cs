using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunBehaviourScript : MonoBehaviour
{

    public GameObject bulletPref;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject instBullet = Instantiate(bulletPref, transform.position, Quaternion.identity);
            Destroy(instBullet, 3f);
        }
    }
}
