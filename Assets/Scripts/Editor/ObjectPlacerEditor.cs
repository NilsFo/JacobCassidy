using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPlacerScript))]
public class ObjectPlacerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ObjectPlacerScript script = (ObjectPlacerScript) target;
        if (GUILayout.Button("Align objects to pixel grid"))
        {
            script.AlignObjects();
        }

        if (GUILayout.Button("Set Z-Coordinates"))
        {
            script.SetZCoord();
        }
    }
}