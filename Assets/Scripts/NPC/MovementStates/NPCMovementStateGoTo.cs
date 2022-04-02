using System.Collections.Generic;
using UnityEngine;

public class NPCMovementStateGoTo : NPCMovementStateBase
{
    protected Vector2 goToTarget;

    public NPCMovementStateGoTo(NPCMovementAI sourceNPC, Vector2 goToTarget) : base(sourceNPC, false)
    {
        this.goToTarget = goToTarget;
    }

    public override string GetUIDescription()
    {
        return "Moving to: " + goToTarget;
    }

    public override Vector2 GetPathfindingTargetLocation()
    {
        Vector3 pos = goToTarget;
        return new Vector2(pos.x, pos.y);
    }

    public override bool ShouldMove()
    {
        // TODO check distance?
        return true;
    }

    public override List<string> GetAdditionalEditorLines()
    {
        return new List<string>();
    }

    public override void OnPathComplete()
    {
        base.OnPathComplete();
        sourceNPC.SetMovementStateWaitAtLocation(goToTarget);
    }
}