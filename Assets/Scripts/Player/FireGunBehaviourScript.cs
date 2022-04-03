using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunBehaviourScript : MonoBehaviour
{

    [SerializeField] private PlayerStateBehaviourScript playerStateBehaviourScript;
    [SerializeField] private SpellStateBehaviourScript spellStateBehaviourScript;
    
    public BulletBehaviourScript bulletPref;
    public BulletTrailBehaviour bulletTrailPrefab;
    public Transform aimDummy;
    
    public float speed = 10f;

    public float fireDelay = 0.2f;

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

            if (_direction.magnitude > 0)
            {
                if (input.Player.Fire.triggered)
                {
                    if (playerStateBehaviourScript.ChangeCurrentAmmo(-1))
                    {
                        Fire(_direction);
                    }
                    else
                    {
                        //TODO Click Sound Trigger
                        playerStateBehaviourScript.ReloadAmmo();
                    }
                }
                else if (input.Player.Spell.triggered)
                {
                    spellStateBehaviourScript.CastSpell(gameObject, _direction);
                }
            }   
        } else {
            fireTime -= Time.deltaTime;
        }
    }
    
    private void Fire(Vector2 direction) {

        BulletBehaviourScript instBullet = Instantiate(bulletPref, transform.position, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x)));
        Rigidbody2D instBulletRB = instBullet.GetComponent<Rigidbody2D>();

        instBulletRB.AddForce(direction * speed, ForceMode2D.Force);

        Destroy(instBullet, 3f);
        fireTime = fireDelay;

        // Knockback
        playerMovementBehaviour.Knockback(aimDummy.position, 150);
        
        // Disable Movement
        playerMovementBehaviour.movementCooldown = fireDelay;

        // Gun Trail
        var bulletTrail = Instantiate(bulletTrailPrefab, transform.position, Quaternion.identity);
        bulletTrail.bullet = instBullet;
        bulletTrail.startPos = transform.position;

        //TODO Gun Sound Trigger
    }
}
