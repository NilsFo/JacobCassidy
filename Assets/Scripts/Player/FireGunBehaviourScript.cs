using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunBehaviourScript : MonoBehaviour
{

    [SerializeField] private PlayerStateBehaviourScript _playerStateBehaviourScript;
    
    public BulletBehaviourScript bulletPref;
    public BulletTrailBehaviour bulletTrailPrefab;
    public Transform aimDummy;
    
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

            if (input.Player.Fire.triggered && _direction.magnitude > 0)
            {
                if (_playerStateBehaviourScript.ChangeCurrentAmmo(-1)) {
                    Fire(_direction);
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
            var enemy = hit.collider.transform.parent.GetComponent<EnemyBehaviourScript>();
            if (enemy != null) {
                enemy.ChangeCurrentHealth(-1);
            }
            hitPos = hit.point;
            hitPos.z = -10;
            // TODO hit objects etc
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

        //TODO Gun Sound Trigger
    }
}
