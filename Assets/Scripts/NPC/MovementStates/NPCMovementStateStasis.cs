using System.Collections.Generic;
using UnityEngine;

public class NPCMovementStateStasis : NPCMovementStateBase
{
    public NPCMovementStateStasis(NPCMovementAI sourceNPC) : base(sourceNPC, false)
    {
    }

    public override string GetUIDescription()
    {
        return "In stasis.";
    }

    public override Vector2 GetPathfindingTargetLocation()
    {
        return sourceNPC.transform.position;
    }

    public override bool ShouldMove()
    {
        return false;
    }

    public override List<string> GetAdditionalEditorLines()
    {
        return new List<string>();
    }
}