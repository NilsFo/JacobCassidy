using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NPCMovementAI))]
[CanEditMultipleObjects]
public class NPCMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        NPCMovementAI movingNPC = (NPCMovementAI) target;

        if (!EditorApplication.isPlaying)
        {
            EditorGUILayout.HelpBox("Change to play mode for movement integration!", MessageType.Info);
            return;
        }

        // Printing pathing info
        EditorGUILayout.LabelField("\n==== PATHING INFO ===\n");
        EditorGUILayout.LabelField("Waypoint Progress: ",
            movingNPC.CurrentWaypointIndexToTarget + 1 + "/" + movingNPC.GetWaypointCount());

        EditorGUILayout.LabelField("Current State: " + movingNPC.CurrentMovementState);
        
        if (movingNPC.GetWaypointCount() <= 0)
        {
            EditorGUILayout.HelpBox("No path available!", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.LabelField("======= GENERAL INFO =======");
            EditorGUILayout.LabelField("Waypoint Distance (movement target): ",
                movingNPC.GetDistanceToNextWaypoint().ToString(CultureInfo.InvariantCulture));
            EditorGUILayout.LabelField("Target Distance: ",
                movingNPC.GetDistanceToMovementTarget().ToString(CultureInfo.InvariantCulture));
            EditorGUILayout.LabelField("Reached Target: ", movingNPC.ReachedPath.ToString());

            EditorGUILayout.LabelField("======= STATE INFO =======");
            var state = movingNPC.CurrentMovementState;

            EditorGUILayout.LabelField("State: ", state.GetUIDescription());
            EditorGUILayout.LabelField("Velocity Modifier: ",
                state.GetVelocityModifier().ToString(CultureInfo.InvariantCulture));
            EditorGUILayout.LabelField("Velocity: " + movingNPC.movementSpeed,
                "Modified: " + movingNPC.movementSpeed * state.GetVelocityModifier());
            EditorGUILayout.LabelField("Walking Location: ",
                state.GetPathfindingTargetLocation().ToString());
            EditorGUILayout.LabelField("Should move: ", state.ShouldMove().ToString());
            EditorGUILayout.LabelField("Allowed to move aside: ", state.AllowMovingAsidePlayer.ToString());

            List<string> additionalLines = state.GetAdditionalEditorLines();
            if (additionalLines != null && additionalLines.Count > 0)
            {
                EditorGUILayout.LabelField("==============");
                foreach (string line in additionalLines)
                {
                    EditorGUILayout.LabelField(line);
                }
            }
        }

        EditorGUILayout.LabelField("======= MOVEMENT DEBUG ACTIONS =======");
        

        EditorGUILayout.LabelField("======= MOVEMENT ORDERS =======");
        if (GUILayout.Button("Order: Move to player"))
        {
            movingNPC.SetMovementStateMoveToPlayer();
        }
        if (GUILayout.Button("Order: Follow Player"))
        {
            movingNPC.SetMovementStateFollowPlayer();
        }

        if (GUILayout.Button("Order: Patrol to player"))
        {
            movingNPC.SetMovementStatePatrolToPlayer();
        }

        if (GUILayout.Button("Order: Wait here"))
        {
            movingNPC.SetMovementStateWaitHere();
        }
        

        if (GUILayout.Button("Order: Enter Stasis"))
        {
            movingNPC.SetMovementStateStasis();
        }
    }
}