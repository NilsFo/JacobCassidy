using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class MovementAnimator : MonoBehaviour
{
    public enum FacingDirection : UInt16
    {
        Unknown,
        North,
        East,
        South,
        West
    }

    [Header("World Hookup")] public Animator myMovementAnimator;

    [Header("Movement Parameters")] public bool playing = true;
    public Vector2 velocity;
    private Vector2 orientationIdle;

    [Header("Animation Event")] public UnityEvent onAnimationEvent;

    //[Header("Moving Dust Particle")] public List<DustParticleSpawnBehavior> dustSpawnOrigins;
    public bool spawnDustNextFrame;

    private void Awake()
    {
        // Checking if animator has animations
        Debug.Assert(ContainsParam("test"), gameObject);
        Debug.Assert(ContainsParam("Velocity_Horizontal"), gameObject);
        Debug.Assert(ContainsParam("Velocity_Vertical"), gameObject);
        Debug.Assert(ContainsParam("Idle_Horizontal"), gameObject);
        Debug.Assert(ContainsParam("Idle_Vertical"), gameObject);
        Debug.Assert(ContainsParam("Speed"), gameObject);

        // Checking if fields exist
        Debug.Assert(myMovementAnimator != null, gameObject);

        // Init
        orientationIdle = new Vector2(0f, 0f);

        // Setting up Events
        if (onAnimationEvent == null) onAnimationEvent = new UnityEvent();
    }

    private void Update()
    {
        if (playing && myMovementAnimator.enabled)
        {
            if (velocity.sqrMagnitude > 0.01f)
            {
                orientationIdle = velocity * 2;
            }

            myMovementAnimator.SetFloat("Velocity_Horizontal", velocity.x);
            myMovementAnimator.SetFloat("Velocity_Vertical", velocity.y);
            myMovementAnimator.SetFloat("Idle_Horizontal", orientationIdle.x);
            myMovementAnimator.SetFloat("Idle_Vertical", orientationIdle.y);
            myMovementAnimator.SetFloat("Speed", velocity.sqrMagnitude);
        }
    }

    public float GetFacingAngle()
    {
        Vector2 oneDirection = orientationIdle.normalized;
        var angle = Mathf.Atan2(oneDirection.x, oneDirection.y) * Mathf.Rad2Deg;
        return angle;
    }

    public FacingDirection GetFacing()
    {
        float angle = GetFacingAngle();
        return GetFacing(angle);
    }

    public FacingDirection GetFacing(float angle)
    {
        if (IsBetweenRange(angle, -135, -44))
        {
            return FacingDirection.West;
        }
        else if (IsBetweenRange(angle, -45, 44))
        {
            return FacingDirection.North;
        }
        else if (IsBetweenRange(angle, 45, 134))
        {
            return FacingDirection.East;
        }
        else if (IsBetweenRange(angle, -134, -180) || IsBetweenRange(angle, 135, 180))
        {
            return FacingDirection.South;
        }
        else
        {
            Debug.LogWarning("Warning: Unknown entity orientation detected at: " + angle + "°!", gameObject);
            return FacingDirection.Unknown;
        }
    }

    public void SetFacing(FacingDirection newDirection)
    {
        // Note: This only applies when the object in question is standing still

        Vector2 oneDirection = new Vector2();
        switch (newDirection)
        {
            case FacingDirection.North:
                oneDirection.y = 1;
                break;
            case FacingDirection.East:
                oneDirection.x = 1;
                break;
            case FacingDirection.South:
                oneDirection.y = -1;
                break;
            case FacingDirection.West:
                oneDirection.x = -1;
                break;
            default:
                Debug.LogWarning("Unknown facing direction. Cannot set: " + newDirection, gameObject);
                break;
        }

        orientationIdle = oneDirection;
    }

    public void SetFacing(GameObject objectToLookAt)
    {
        SetFacing(objectToLookAt.transform);
    }

    public void SetFacing(Transform targetToLookAt)
    {
        Vector2 oneDirection = targetToLookAt.position - transform.position;
        oneDirection = oneDirection.normalized;
        float angle = Mathf.Atan2(oneDirection.x, oneDirection.y) * Mathf.Rad2Deg;
        FacingDirection direction = GetFacing(angle);
        SetFacing(direction);
    }

    private bool ContainsParam(string name)
    {
        foreach (AnimatorControllerParameter param in myMovementAnimator.parameters)
        {
            if (param.name == name) return true;
        }

        return false;
    }

    private static bool IsBetweenRange(float value, float floor, float ceil)
    {
        return value >= Mathf.Min(floor, ceil) && value <= Mathf.Max(floor, ceil);
    }


    # region Animation Events

    public void SpawnDustParticles()
    {
        /*foreach (DustParticleSpawnBehavior spawner in dustSpawnOrigins)
        {
            // print("SPAWNING DUST");
            spawner.Spawn();
        }*/
    }

    public delegate void OnCustomAnimationEvent(string eventID);

    public event OnCustomAnimationEvent CustomAnimationEvent;

    public virtual void OnCustomAnimation(string eventID)
    {
        //  Define Function trigger and protect the event for not null;
        if (CustomAnimationEvent != null)
        {
            CustomAnimationEvent(eventID);
        }
    }

    #endregion
}