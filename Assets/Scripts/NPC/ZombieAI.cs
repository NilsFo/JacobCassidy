using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public enum ZombieState : UInt16
    {
        Waiting,
        Attacking,
        ChasePlayer,
        FollowCultist,
        GoToLastKnownLocation,
        GoToSpawn,
        Roaming,
        Dead
    }

    public GameObject myCultist;

    public EnemyBehaviourScript enemyBehaviourScript;
    public NPCMovementAI myMovement;
    private Vector2 myCreationPoint;
    public float playerDetectionDistance = 3f;
    public float playerDetectionDistanceHightened = 8;
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
        myMovement.SetMovementStateStasis();
        player = FindObjectOfType<PlayerMovementBehaviour>();
        playerState = FindObjectOfType<PlayerStateBehaviourScript>();
    }

    private void OnEnable()
    {
        currentState = ZombieState.Waiting;
        _lastState = ZombieState.Waiting;
        enemyBehaviourScript.OnDamageTaken += EnemyBehaviourScriptOnOnDamageTaken;
    }

    private void EnemyBehaviourScriptOnOnDamageTaken(GameObject self, float damage)
    {
        currentState = ZombieState.GoToLastKnownLocation;
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
            case ZombieState.ChasePlayer:
                if (IsInMeleeRange() && canSeePlayer)
                {
                    currentState = ZombieState.Attacking;
                }

                break;
            case ZombieState.GoToLastKnownLocation:
                if (canSeePlayer)
                {
                    currentState = ZombieState.ChasePlayer;
                }
                else
                {
                    if (myMovement.ReachedPath)
                    {
                        currentState = ZombieState.GoToSpawn;
                    }
                }

                break;
        }

        // Checking if Should detect player
        if (currentState == ZombieState.Waiting || currentState == ZombieState.GoToSpawn ||
            currentState == ZombieState.Roaming || currentState == ZombieState.FollowCultist ||
            currentState == ZombieState.GoToLastKnownLocation)
        {
            if (canSeePlayer)
            {
                currentState = ZombieState.ChasePlayer;
            }
        }

        if (currentState == ZombieState.ChasePlayer)
        {
            if (!canSeePlayer)
            {
                currentState = ZombieState.GoToLastKnownLocation;
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
            case ZombieState.ChasePlayer:
                myMovement.SetMovementStateFollowPlayer();
                break;
            case ZombieState.GoToSpawn:
                myMovement.SetMovementStateMoveTo(myCreationPoint);
                break;
            case ZombieState.Dead:
                myMovement.SetMovementStateStasis();
                break;
            case ZombieState.GoToLastKnownLocation:
                myMovement.SetMovementStateMoveToPlayer();
                break;
            case ZombieState.FollowCultist:
                if (myCultist == null)
                {
                    currentState = ZombieState.Roaming;
                }
                else
                {
                    myMovement.SetStateFollow(myCultist);
                }

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

    public void MakeAttack()
    {
        var player = FindObjectOfType<PlayerMovementBehaviour>();
        if (attackCollider.IsTouching(player.GetComponent<Collider2D>()))
        {
            player.Knockback(transform.position, knockbackForce);
            playerState.ChangeCurrentHealth(-1);
        }
    }

    public void EndAttack()
    {
        currentState = ZombieState.Roaming;
    }

    public void UpdateCanSeePlayer()
    {
        canSeePlayer = false;
        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist < GetDetectionDistance())
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

    public void Death()
    {
        currentState = ZombieState.Dead;
        myMovement.myAnimator.myMovementAnimator.SetTrigger("Die");
        var debrisLayer = LayerMask.NameToLayer("Debris");
        gameObject.layer = debrisLayer;
        foreach (var componentsInChild in gameObject.GetComponentsInChildren<Collider2D>())
        {
            componentsInChild.gameObject.layer = debrisLayer;
        }
    }

    public void DespawnAfterDeath()
    {
        Destroy(gameObject);
    }

    public float GetDetectionDistance()
    {
        if (currentState == ZombieState.ChasePlayer || currentState == ZombieState.GoToLastKnownLocation)
        {
            return playerDetectionDistanceHightened;
        }

        return playerDetectionDistance;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            //Debug.DrawLine(sourceNPC.transform.position, roamingOrigin);
            Vector3 wireOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            Handles.DrawWireDisc(wireOrigin, Vector3.forward, meleeRange);
            Handles.DrawWireDisc(wireOrigin, Vector3.forward, GetDetectionDistance());
            Handles.Label(transform.position, "State: " + currentState);
        }
#endif
    }

    public void SetStunTime(float time)
    {
        myMovement.stunnedTimer = time;
    }
    
}