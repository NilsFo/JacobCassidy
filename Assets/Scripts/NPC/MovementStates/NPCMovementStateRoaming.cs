using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCMovementStateRoaming : NPCMovementStateBase
{
    // Params
    protected Vector2 roamingOrigin;
    protected float nextRoamingTimer;
    protected float nextRoamingTimerJitter;
    protected float roamingRadius;

    // Getters & Setters

    public Vector2 RoamingOrigin
    {
        get => roamingOrigin;
        set => roamingOrigin = value;
    }

    public float RoamingRadius
    {
        get => roamingRadius;
        set => roamingRadius = value;
    }

    public float RoamingMinDistance => RoamingRadius / 2.0f;

    // Fields
    private Vector2 currentRoamTarget;
    private float currentRoamingTimer = 0f;
    private float currentRoamingTimerTarget = float.MaxValue;

    public NPCMovementStateRoaming(NPCMovementAI sourceNPC, Vector2 roamingOrigin, float roamingRadius = 0.8f,
        float nextRoamingTimer = 5f, float nextRoamingTimerJitter = 5f) : base(sourceNPC, false)
    {
        this.roamingOrigin = roamingOrigin;
        this.nextRoamingTimer = nextRoamingTimer;
        this.nextRoamingTimerJitter = nextRoamingTimerJitter;
        this.roamingRadius = roamingRadius;
        // TODO also set look direction

        currentRoamTarget = new Vector2(roamingOrigin.x, roamingOrigin.y);
        PrepareNextTarget();
    }

    public override string GetUIDescription()
    {
        return "Roaming around " + roamingOrigin + " with radius " + roamingRadius + ".";
    }

    public override Vector2 GetPathfindingTargetLocation()
    {
        return new Vector2(currentRoamTarget.x, currentRoamTarget.y);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        currentRoamingTimer += deltaTime;
    }

    public override void DrawGizmos()
    {
        #if UNITY_EDITOR
        
        base.DrawGizmos();
        Debug.DrawLine(sourceNPC.transform.position, roamingOrigin);
        Vector3 wireOrigin = new Vector3(roamingOrigin.x, roamingOrigin.y, sourceNPC.transform.position.z - 1);
        UnityEditor.Handles.DrawWireDisc(wireOrigin, Vector3.forward, roamingRadius);
        
        #endif
    }

    public override void OnPathComplete()
    {
        base.OnPathComplete();
        PrepareNextTarget();
    }

    public void PrepareNextTarget()
    {
        var r = Random.insideUnitCircle * roamingRadius;
        Vector2 currentPosition = sourceNPC.transform.position;
        currentRoamTarget = new Vector2(currentPosition.x, currentPosition.y);

        int iterations = 0;
        while (Vector2.Distance(currentRoamTarget, currentPosition) < RoamingMinDistance && iterations < 100)
        {
            iterations++;
            // Debug.Log("Searching for new target. Iterations: " + iterations);
            currentRoamTarget = new Vector2(roamingOrigin.x + r.x, roamingOrigin.y + r.y);
        }

        currentRoamingTimer = 0;
        currentRoamingTimerTarget = nextRoamingTimer + nextRoamingTimerJitter * Random.Range(0f, 1f);
    }

    public override bool ShouldMove()
    {
        return currentRoamingTimer >= currentRoamingTimerTarget;
    }

    public override List<string> GetAdditionalEditorLines()
    {
        return new List<string>
        {
            "Origin Location: " + roamingOrigin,
            "Roaming radius: " + roamingRadius,
            "Cooldown Timer: " + currentRoamingTimer + "/" + currentRoamingTimerTarget,
            "Cooldown Timer Base: " + nextRoamingTimer + " + " + nextRoamingTimerJitter + " jitter"
        };
    }
}