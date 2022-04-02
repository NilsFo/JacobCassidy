using System.Collections.Generic;
using UnityEngine;

public class NPCMovementStatePatrol : NPCMovementStateBase
{
    protected Vector2 sourcePosition;
    protected Vector2 targetPosition;

    protected bool onWayBack = false;

    public NPCMovementStatePatrol(NPCMovementAI sourceNPC, Vector2 sourcePosition, Vector2 targetPosition) :
        base(sourceNPC, false)
    {
        this.targetPosition = targetPosition;
        this.sourcePosition = sourcePosition;
        onWayBack = false;
        // TODO also set look direction
    }

    public override string GetUIDescription()
    {
        return "Patrolling between " + sourcePosition + " and " + targetPosition + ".";
    }

    public override Vector2 GetPathfindingTargetLocation()
    {
        if (onWayBack)
        {
            return sourcePosition;
        }

        return targetPosition;
    }

    public override bool ShouldMove()
    {
        return true;
    }

    public override List<string> GetAdditionalEditorLines()
    {
        return new List<string> {"On way back: " + onWayBack};
    }

    public override void OnPathComplete()
    {
        onWayBack = !onWayBack;
    }
}