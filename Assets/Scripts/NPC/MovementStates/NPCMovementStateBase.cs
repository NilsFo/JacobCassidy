using System.Collections.Generic;
using UnityEngine;

public abstract class NPCMovementStateBase
{
    protected NPCMovementAI sourceNPC;
    protected float velocityModifier;
    protected bool allowMovingAside;

    public NPCMovementAI SourceNpc => sourceNPC;
    public bool AllowMovingAsidePlayer => allowMovingAside;

    public float VelocityModifier
    {
        protected get => velocityModifier;
        set => velocityModifier = value;
    }

    protected NPCMovementStateBase(NPCMovementAI sourceNPC, bool allowMovingAside)
    {
        this.sourceNPC = sourceNPC;
        this.allowMovingAside = allowMovingAside;
        VelocityModifier = 1.0f;
        Debug.Assert(sourceNPC != null, sourceNPC.gameObject);
    }

    public virtual void Update(float deltaTime)
    {
    }

    public virtual void OnPathComplete()
    {
    }

    public virtual void DrawGizmos()
    {
    }

    public abstract string GetUIDescription();

    public abstract Vector2 GetPathfindingTargetLocation();

    public abstract bool ShouldMove();

    public abstract List<string> GetAdditionalEditorLines();

    public virtual float GetVelocityModifier()
    {
        return VelocityModifier;
    }
}