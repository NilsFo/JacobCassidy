using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunBehaviourScript : MonoBehaviour
{

    [SerializeField] private PlayerStateBehaviourScript _playerStateBehaviourScript;
    
    public GameObject bulletPref;
    public float speed = 10f;
    
    public float fireDelay = 0.5f;
    
    private float fireTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
        fireTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTime <= 0)
        {
            var horizontal2Input = Input.GetAxisRaw("Horizontal2");
            var vertical2Input = Input.GetAxisRaw("Vertical2");
        
            var _direction = new Vector2(horizontal2Input, vertical2Input);
            _direction = _direction.normalized;

            if (Input.GetButtonDown("Fire1") && _direction.magnitude > 0)
            {
                if (_playerStateBehaviourScript.ChangeCurrentAmmo(-1))
                {
                    GameObject instBullet = Instantiate(bulletPref, transform.position, Quaternion.identity);
                    Rigidbody2D instBulletRB = instBullet.GetComponent<Rigidbody2D>();
                
                    instBulletRB.AddForce(_direction * speed, ForceMode2D.Force);

                    Destroy(instBullet, 3f);
                    fireTime = fireDelay;
                    
                    //TODO Gun Sound Trigger
                }
                else
                {
                    //TODO Click Sound Trigger
                    //TODO Trigger Reload
                }
            }   
        }
    }
}
