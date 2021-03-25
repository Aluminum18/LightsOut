using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameBoard))]
public class GameBoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myTarget = (GameBoard)target;
        if (GUILayout.Button("SetupLights"))
        {
            myTarget.SetupLights();
        }

        if (GUILayout.Button("RemoveSetup"))
        {
            myTarget.RemoveLightNeighbor();
        }
    }
}
