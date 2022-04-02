using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Pathfinding;
using UnityEditor;
using UnityEngine.Events;

public class NPCMovementAI : MonoBehaviour
{
    public enum MovementState : UInt16
    {
        Stasis,
        Waiting,
        FollowPlayer,
        MoveToPlayer,
        PatrolToPlayer,
        Roaming
    }

    [Header("General Movement Settings")] public bool paused = false;
    // public MovementAnimator myAnimator;
    // TODO add

    [Header("Movement Params")] public float movementSpeed = 1500f;
    private Rigidbody2D rb;

    [Header("Pathfinding Parameters")] public float nextWaypointDistanceTolerance = .1337f;
    private Path currentPathToTarget;
    private int currentWaypointIndexToTarget = 0;
    [SerializeField] private bool reachedPath = false;
    private bool reachedPathEventFired = false;
    public float updatePathFrequency = 0.5f;
    private float updatePathFrequencyCurrent = 0;
    public Seeker movementTargetSeeker;
    [SerializeField] private int nodeSkippingCount = 3;

    [Header("In-Game Config")] public MovementState initialMovementState = MovementState.Waiting;
    private NPCMovementStateBase currentMovementState;

    [Header("Unity Callbacks")] public UnityEvent onPathCompleted;

    // Properties
    public int CurrentWaypointIndexToTarget => currentWaypointIndexToTarget;
    public bool ReachedPath => reachedPath;
    public NPCMovementStateBase CurrentMovementState => currentMovementState;

    // Private fields
    private PlayerMovementBehaviour player;
    private int pathsCalculatedCount = 0;
    private Queue<Vector2> velocityHistory;
    private Vector2 _lastFramePosition;

    private void Awake()
    {
        currentWaypointIndexToTarget = 0;
        reachedPath = false;
        currentMovementState = null;
        rb = GetComponentInChildren<Rigidbody2D>();
        _lastFramePosition = rb.position;
        reachedPathEventFired = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        reachedPathEventFired = false;
        player = FindObjectOfType<PlayerMovementBehaviour>();
        pathsCalculatedCount = 0;


        // Handling initial movement state
        switch (initialMovementState)
        {
            case MovementState.Stasis:
                SetMovementStateStasis();
                break;
            case MovementState.Waiting:
                SetMovementStateWaitHere();
                break;
            case MovementState.FollowPlayer:
                SetMovementStateFollowPlayer();
                break;
            case MovementState.MoveToPlayer:
                SetMovementStateMoveToPlayer();
                break;
            case MovementState.PatrolToPlayer:
                SetMovementStatePatrolToPlayer();
                break;
            case MovementState.Roaming:
                SetMovementStateRoaming();
                break;
            default:
                Debug.LogWarning("Initial movement state not implemented yet: " + initialMovementState, gameObject);
                SetMovementStateWaitHere();
                break;
        }

        UpdatePath();
    }

    private void UpdatePath()
    {
        if (!currentMovementState.ShouldMove() && pathsCalculatedCount > 0)
            return;


        if (movementTargetSeeker.IsDone())
        {
            movementTargetSeeker.StartPath(rb.position, currentMovementState.GetPathfindingTargetLocation(),
                OnPathCalculated);
            pathsCalculatedCount++;
        }
    }

    private void OnPathCalculated(Path p)
    {
        if (p.error)
        {
            Debug.LogWarning("Failed to find a movement path!", gameObject);
            // TODO handle
            return;
        }

        currentPathToTarget = p;
        currentWaypointIndexToTarget = 0;

        List<Vector3> newPath = p.vectorPath;
        if (newPath.Count >= 1)
        {
            Vector2 currentPos = transform.position;
            Vector2 targetPos = currentMovementState.GetPathfindingTargetLocation();
            float currentDistance = Vector2.Distance(currentPos, targetPos);

            int checkedNodes = Mathf.Max(Mathf.Min(nodeSkippingCount, newPath.Count), 1) - 1;
            for (int i = 0; i < checkedNodes; i++)
            {
                Vector2 currentPoint = newPath[i];
                float pointDistance = Vector2.Distance(currentPoint, targetPos);
                if (pointDistance > currentDistance)
                {
                    currentWaypointIndexToTarget = i + 1;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentMovementState.ShouldMove())
            currentPathToTarget = null;

        Vector2 targetPos = currentMovementState.GetPathfindingTargetLocation();
        if (currentPathToTarget != null && !reachedPath)
        {
            Debug.DrawLine(rb.position, targetPos, Color.magenta);
        }

        updatePathFrequencyCurrent -= Time.deltaTime;
        if (updatePathFrequencyCurrent < 0)
        {
            updatePathFrequencyCurrent = updatePathFrequency;
            UpdatePath();
        }

        // Updating the movement state
        currentMovementState.Update(Time.deltaTime);
    }

    public void ForceUpdatePath()
    {
        // Setting the timer to 0, so it will be updated next 'Update'
        // Done to avoid multiple calculations per frame
        updatePathFrequencyCurrent = 0;
    }

    private void FixedUpdate()
    {
        // TODO update animations here
        if (paused)
        {
            return;
        }

        if (currentPathToTarget == null)
        {
            return;
        }
        
        if (currentWaypointIndexToTarget >= currentPathToTarget.vectorPath.Count)
        {
            reachedPath = true;
            if (!reachedPathEventFired)
            {
                reachedPathEventFired = true;
                OnPathReached();
            }
            currentPathToTarget = null;

            return;
        }

        if (!currentMovementState.ShouldMove())
        {
            return;
        }

        reachedPath = false;
        reachedPathEventFired = false;
        Vector2 direction = (Vector2) currentPathToTarget.vectorPath[currentWaypointIndexToTarget] - rb.position;
        direction = direction.normalized;

        float velocityModifier = CurrentMovementState.GetVelocityModifier();

        float velocity = movementSpeed * velocityModifier;
        Vector2 force = direction * velocity * Time.fixedDeltaTime;

        // Adding force to move the entity
        rb.AddForce(force);

        float distanceToWayPoint = GetDistanceToNextWaypoint();
        if (distanceToWayPoint < nextWaypointDistanceTolerance)
        {
            currentWaypointIndexToTarget++;
        }
    }

    public float GetDistanceToNextWaypoint()
    {
        if (paused)
        {
            return float.NaN;
        }

        if (currentPathToTarget == null || !currentMovementState.ShouldMove())
        {
            return float.NaN;
        }

        if (currentWaypointIndexToTarget >= currentPathToTarget.vectorPath.Count)
        {
            return float.NaN;
        }

        return Vector2.Distance(rb.position, currentPathToTarget.vectorPath[currentWaypointIndexToTarget]);
    }

    public float GetDistanceToMovementTarget()
    {
        if (currentPathToTarget == null || paused || !currentMovementState.ShouldMove())
        {
            return float.NaN;
        }

        return Vector2.Distance(rb.position, currentMovementState.GetPathfindingTargetLocation());
    }

    public int GetWaypointCount()
    {
        if (currentPathToTarget == null)
        {
            return 0;
        }

        return currentPathToTarget.vectorPath.Count;
    }

    private void OnDrawGizmos()
    {
        if (currentMovementState != null)
        {
            currentMovementState.DrawGizmos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (currentMovementState != null)
        {
            currentMovementState.DrawGizmos();
        }
    }

    # region listeners

    private void OnPathReached()
    {
        onPathCompleted.Invoke();
        currentMovementState.OnPathComplete();
    }

    #endregion


    # region Setters for Movement States

    public void SetStateFollow(GameObject followTarget)
    {
        SetMovementState(new NPCMovementStateFollow(this, followTarget));
    }

    public void SetMovementStateRoaming()
    {
        SetMovementState(new NPCMovementStateRoaming(this, transform.position));
    }

    public void SetMovementStatePatrolToPlayer()
    {
        // TODO use the gamestate to find the player game object
        SetMovementState(new NPCMovementStatePatrol(this, transform.position, player.transform.position));
    }

    public void SetMovementStateMoveToPlayer()
    {
        // TODO use the gamestate to find the player game object
        SetMovementStateMoveTo(player.gameObject);
    }

    public void SetMovementStateMoveTo(GameObject goToTarget)
    {
        SetMovementStateMoveTo(goToTarget.transform.position);
    }

    public void SetMovementStateMoveTo(Vector2 goToTarget)
    {
        SetMovementState(new NPCMovementStateGoTo(this, goToTarget));
    }

    public void SetMovementStateFollowPlayer()
    {
        // TODO use the game state to find the player game object
        SetStateFollow(player.gameObject);
    }

    public void SetMovementStateStasis()
    {
        SetMovementState(new NPCMovementStateStasis(this));
    }

    public void SetMovementStateWaitHere()
    {
        SetMovementStateWaitAtLocation(transform.position);
    }

    public void SetMovementStateWaitAtLocation(Vector2 location)
    {
        SetMovementState(new NPCMovementStateWait(this, location));
    }

    public void SetMovementState(NPCMovementStateBase newState)
    {
        NPCMovementStateBase oldState = currentMovementState;
        currentMovementState = newState;
        UpdatePath();
        // TODO fire a listener here?
    }

    #endregion
}