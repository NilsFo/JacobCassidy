using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunBehaviourScript : MonoBehaviour
{

    [SerializeField] private PlayerStateBehaviourScript _playerStateBehaviourScript;
    
    public BulletBehaviourScript bulletPref;
    public BulletTrailBehaviour bulletTrailPrefab;
    public Transform aimDummy;
    
    public float speed = 10f;

    public float fireDelay = 0.5f;


    private float fireTime;

    private MainInputActionsSettings input;
    
    public PlayerMovementBehaviour playerMovementBehaviour;
    
    // Start is called before the first frame update
    void Start()
    {
        fireTime = 0f;
        input = FindObjectOfType<GameStateBehaviourScript>().mainInputActions;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTime <= 0)
        {
            var aimDummyPos = aimDummy.transform.localPosition;
            var horizontal2Input = aimDummyPos.x;
            var vertical2Input = aimDummyPos.y;
        
            var _direction = new Vector2(horizontal2Input, vertical2Input);
            _direction = _direction.normalized;

            if (input.Player.Fire.triggered && _direction.magnitude > 0)
            {
                if (_playerStateBehaviourScript.ChangeCurrentAmmo(-1))
                {
                    BulletBehaviourScript instBullet = Instantiate(bulletPref, transform.position, Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan2(_direction.y, _direction.x)));
                    Rigidbody2D instBulletRB = instBullet.GetComponent<Rigidbody2D>();
                
                    instBulletRB.AddForce(_direction * speed, ForceMode2D.Force);

                    Destroy(instBullet, 3f);
                    fireTime = fireDelay;
                    
                    // Knockback
                    playerMovementBehaviour.Knockback(aimDummy.position,150);
                    
                    // Gun Trail
                    var bulletTrail = Instantiate(bulletTrailPrefab, transform.position, Quaternion.identity);
                    bulletTrail.bullet = instBullet;
                    bulletTrail.startPos = transform.position;

                    //TODO Gun Sound Trigger
                }
                else
                {
                    //TODO Click Sound Trigger
                    _playerStateBehaviourScript.ReloadAmmo();
                }
            }   
        } else {
            fireTime -= Time.deltaTime;
        }
    }
}
