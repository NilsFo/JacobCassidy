using System.Collections.Generic;
using UnityEngine;

public class NPCMovementStateWait : NPCMovementStateBase
{
    protected Vector2 waitLocation;

    public NPCMovementStateWait(NPCMovementAI sourceNPC, Vector2 waitLocation) : base(sourceNPC, false)
    {
        this.waitLocation = waitLocation;
        // TODO also set look direction
    }

    public override string GetUIDescription()
    {
        return "Waiting at position: " + waitLocation;
    }

    public override Vector2 GetPathfindingTargetLocation()
    {
        return new Vector2(waitLocation.x, waitLocation.y);
    }

    public override bool ShouldMove()
    {
        return Vector2.Distance(sourceNPC.transform.position, waitLocation) > 0.15;
    }

    public override List<string> GetAdditionalEditorLines()
    {
        return new List<string>();
    }
}