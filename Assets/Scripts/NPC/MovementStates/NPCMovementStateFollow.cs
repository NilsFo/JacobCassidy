using System.Collections.Generic;
using UnityEngine;

public class NPCMovementStateFollow : NPCMovementStateBase
{
    protected GameObject followTarget;
    protected NPCMovementStateRoaming stateRoaming;
    protected bool allowRoaming;
    protected float roamingDistanceThreshold;

    private bool isRoaming = false;

    public NPCMovementStateFollow(NPCMovementAI sourceNPC, GameObject followTarget, bool allowRoaming = true,
        float roamingDistanceThreshold = 1.42f) : base(sourceNPC, false)
    {
        stateRoaming = new NPCMovementStateRoaming(sourceNPC, followTarget.transform.position,
            roamingRadius: roamingDistanceThreshold * 0.755536f);
        stateRoaming.VelocityModifier = 0.5f;
        this.followTarget = followTarget;
        this.allowRoaming = allowRoaming;
        this.roamingDistanceThreshold = roamingDistanceThreshold;
        // TODO also set look direction
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        // Updating the 'roaming' state
        bool roamingCache = isRoaming;
        isRoaming = allowRoaming && GetDistanceToFollowTarget() < roamingDistanceThreshold;

        // Checking against cache
        if (roamingCache != isRoaming)
        {
            sourceNPC.ForceUpdatePath();
        }

        // Applying the 'roaming' state
        stateRoaming.RoamingOrigin = followTarget.transform.position;
        if (IsRoaming())
        {
            stateRoaming.Update(deltaTime);
        }
        else
        {
            stateRoaming.PrepareNextTarget();
        }
    }

    public override void OnPathComplete()
    {
        base.OnPathComplete();

        stateRoaming.RoamingOrigin = sourceNPC.transform.position;
        if (IsRoaming())
        {
            stateRoaming.OnPathComplete();
        }
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();
        if (IsRoaming())
        {
            stateRoaming.DrawGizmos();
        }
    }

    public override string GetUIDescription()
    {
        if (IsRoaming())
        {
            return "Roaming while following.";
        }

        PlayerMovement playerMovementBehaviour =
            followTarget.GetComponentInChildren<PlayerMovement>();
        // TODO use the game state to find the player game object
        if (playerMovementBehaviour != null)
        {
            return "Following the player.";
        }

        return "Following a GameObject that is currently located at: " + followTarget.transform.position;
    }

    public override Vector2 GetPathfindingTargetLocation()
    {
        if (false)//IsRoaming())
        {
            return stateRoaming.GetPathfindingTargetLocation();
        }

        var pos = followTarget.transform.position;
        return new Vector2(pos.x, pos.y);
    }

    public override bool ShouldMove()
    {
        if (IsRoaming())
        {
            return stateRoaming.ShouldMove();
        }

        return true;
    }

    public float GetDistanceToFollowTarget()
    {
        var pos = followTarget.transform.position;
        return Vector2.Distance(sourceNPC.transform.position, new Vector2(pos.x, pos.y));
    }

    public bool IsRoaming()
    {
        return isRoaming;
    }

    public override float GetVelocityModifier()
    {
        if (IsRoaming())
        {
            return stateRoaming.GetVelocityModifier();
        }

        return VelocityModifier;
    }

    public override List<string> GetAdditionalEditorLines()
    {
        var list = new List<string>
        {
            "Distance to target: " + GetDistanceToFollowTarget(),
            "Roaming allowed: " + allowRoaming + " at " + roamingDistanceThreshold,
            "Is Roaming: " + IsRoaming()
        };
        if (IsRoaming())
        {
            list.Add(" -- Roaming --");
            list.AddRange(stateRoaming.GetAdditionalEditorLines());
        }

        return list;
    }
}