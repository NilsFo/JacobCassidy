using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerStateBehaviourScript))]
public class PlayerStateBehaviourScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerStateBehaviourScript playerStateBehaviourScriptEditor = (PlayerStateBehaviourScript) target;
        
        DrawDefaultInspector();
        //This draws the default screen.  You don't need this if you want
        //to start from scratch, but I use this when I'm just adding a button or
        //some small addition and don't feel like recreating the whole inspector.
        if(GUILayout.Button("Punch the Face (-1)"))
        {
            playerStateBehaviourScriptEditor.ChangeCurrentHealth(-1);
        }
    }
}
