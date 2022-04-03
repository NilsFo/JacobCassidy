using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public enum ZombieState : UInt16
    {
        Waiting,
        Attacking,
        GoToPlayer,
        GoToSpawn,
        Roaming,
        Dead
    }

    public EnemyBehaviourScript enemyBehaviourScript;
    public NPCMovementAI myMovement;
    private Vector2 myCreationPoint;
    public float playerDetectionDistance;
    public ZombieState currentState = ZombieState.Waiting;
    private ZombieState _lastState;
    private PlayerMovementBehaviour player;
    private PlayerStateBehaviourScript playerState;
    public float meleeRange = 0.95f;
    [SerializeField] private bool canSeePlayer;
    public int knockbackForce = 200;

    public Collider2D attackCollider;

    // Start is called before the first frame update
    void Start()
    {
        myCreationPoint = transform.position;
        _lastState = currentState;
        myMovement.SetMovementStateStasis();
        player = FindObjectOfType<PlayerMovementBehaviour>();
        playerState = FindObjectOfType<PlayerStateBehaviourScript>();
    }

    private void OnEnable() {
        enemyBehaviourScript.OnDamageTaken += EnemyBehaviourScriptOnOnDamageTaken;
    }
    private void EnemyBehaviourScriptOnOnDamageTaken(GameObject self, float damage) {
        currentState=ZombieState.GoToPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        switch (currentState)
        {
            case ZombieState.GoToSpawn:
                if (myMovement.ReachedPath)
                {
                    currentState = ZombieState.Roaming;
                }

                break;
            case ZombieState.GoToPlayer:
                if (IsInMeleeRange() && canSeePlayer)
                {
                    currentState = ZombieState.Attacking;
                }

                break;
        }

        // Checking if Should detect player
        if (currentState == ZombieState.Waiting || currentState == ZombieState.GoToSpawn ||
            currentState == ZombieState.Roaming)
        {
            if (canSeePlayer)
            {
                currentState = ZombieState.GoToPlayer;
            }
        }

        if (currentState == ZombieState.GoToPlayer)
        {
            if (!canSeePlayer)
            {
                currentState = ZombieState.GoToSpawn;
            }
        }
    }

    public void UpdateState()
    {
        if (currentState == _lastState)
        {
            return;
        }

        _lastState = currentState;
        switch (currentState)
        {
            case ZombieState.Waiting:
                myMovement.SetMovementStateWaitHere();
                break;
            case ZombieState.Attacking:
                BeginAttack();
                break;
            case ZombieState.Roaming:
                myMovement.SetMovementStateRoaming();
                break;
            case ZombieState.GoToPlayer:
                myMovement.SetMovementStateFollowPlayer();
                break;
            case ZombieState.GoToSpawn:
                myMovement.SetMovementStateMoveTo(myCreationPoint);
                break;
            case ZombieState.Dead:
                myMovement.SetMovementStateStasis();
                break;
            default:
                Debug.LogWarning("Unknown movement state for this zombie!", gameObject);
                break;
        }
    }

    private void FixedUpdate()
    {
        UpdateCanSeePlayer();
    }

    public void BeginAttack()
    {
        
        Debug.LogWarning("ZOMBIE ATTACK!");
        myMovement.myAnimator.myMovementAnimator.SetTrigger("Attack");
        myMovement.SetMovementStateWaitHere();
        
        // Rotate attack collider towards player
        Vector3 playerTarget = FindObjectOfType<PlayerMovementBehaviour>().transform.position;
        var dir = playerTarget - transform.position;
        attackCollider.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f);
    }

    public void MakeAttack() {
        var player = FindObjectOfType<PlayerMovementBehaviour>();
        if (attackCollider.IsTouching(player.GetComponent<Collider2D>())) {
            player.Knockback(transform.position, knockbackForce);
            playerState.ChangeCurrentHealth(-1);
        }
    }

    public void EndAttack() {
        
        currentState = ZombieState.Roaming;
    }

    public void UpdateCanSeePlayer()
    {
        canSeePlayer = false;
        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist < playerDetectionDistance)
        {
            canSeePlayer = true;
            var mask = LayerMask.GetMask("NavMeshBlocker");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position,
                distance: dist, layerMask: mask);
            if (hit.collider != null)
            {
                canSeePlayer = false;
            }
        }
    }

    public bool IsInMeleeRange()
    {
        return Vector2.Distance(transform.position, player.transform.position) < meleeRange;
    }

    public void DamageTaken(float damage) {
        
    }

    public void Death() {
        currentState = ZombieState.Dead;
        myMovement.myAnimator.myMovementAnimator.SetTrigger("Die");
        var debrisLayer = LayerMask.NameToLayer("Debris");
        gameObject.layer = debrisLayer;
        foreach (var componentsInChild in gameObject.GetComponentsInChildren<Collider2D>()) {
            componentsInChild.gameObject.layer = debrisLayer;
        }
    }

    public void DespawnAfterDeath() {
        Destroy(gameObject);
    }
}