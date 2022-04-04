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
    
    public float fireDelay = 0.2f;

    private float fireTime;
    public float spellDelay = 0.2f;

    private float spellTime;

    private MainInputActionsSettings input;
    
    public PlayerMovementBehaviour playerMovementBehaviour;

    public AudioSource revolverSound;
    
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

        var layerMask = LayerMask.GetMask(new[] {
            "EnemyHitbox",
            "Objects",
            "NavMeshBlocker"
        });
        float maxRaycastDist = 50f;
        var hit = Physics2D.Raycast(transform.position, direction, maxRaycastDist, layerMask);

        Vector3 hitPos = Vector3.zero;
        if (hit.collider != null) {
            var enemy = hit.collider.transform.GetComponentInParent<EnemyBehaviourScript>();
            if (enemy != null) {
                enemy.ChangeCurrentHealth(-1);
            }
            hitPos = hit.point;
            hitPos.z = -10;
            // TODO More hit objects etc
            var objectHit = hit.collider.transform.GetComponentInParent<DoorBehaviourScript>();
            if (objectHit != null)
            {
                objectHit.DamageHealth(1);
            }
        } else {
            hitPos = new Vector3(transform.position.x + direction.normalized.x * maxRaycastDist, transform.position.y + direction.normalized.y * maxRaycastDist, -5f);
        }
        
        fireTime = fireDelay;

        // Knockback
        playerMovementBehaviour.Knockback(aimDummy.position, 150);
        
        // Disable Movement
        playerMovementBehaviour.movementCooldown = fireDelay;

        // Gun Trail
        var bulletTrail = Instantiate(bulletTrailPrefab, transform.position, Quaternion.identity);

        bulletTrail.hitPos = hitPos;
        bulletTrail.startPos = transform.position;

        revolverSound.pitch = Random.Range(0.8f, 1.2f);
        revolverSound.Play();
    }
}
