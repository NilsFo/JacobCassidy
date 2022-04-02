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
        Roaming
    }

    public NPCMovementAI myMovement;
    private Vector2 myCreationPoint;
    public float playerDetectionDistance;
    public ZombieState currentState = ZombieState.Waiting;
    private ZombieState _lastState;
    private PlayerMovementBehaviour player;
    public float meleeRange = 0.95f;
    [SerializeField] private bool canSeePlayer;

    // Start is called before the first frame update
    void Start()
    {
        myCreationPoint = transform.position;
        _lastState = currentState;
        myMovement.SetMovementStateStasis();
        player = FindObjectOfType<PlayerMovementBehaviour>();
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
                // TODO: Attack animation here
                Debug.LogWarning("ZOMBIE ATTACK!");
                currentState = ZombieState.GoToSpawn;
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
            default:
                Debug.LogWarning("Unknown movement state for this zombie!", gameObject);
                break;
        }
    }

    private void FixedUpdate()
    {
        canSeePlayer = CanSeePlayer();
        //print("state: "+currentState+" - see player "+canSeePlayer);
    }

    public bool CanSeePlayer()
    {
        return Vector2.Distance(transform.position, player.transform.position) < playerDetectionDistance;
    }

    public bool IsInMeleeRange()
    {
        return Vector2.Distance(transform.position, player.transform.position) < meleeRange;
    }
}