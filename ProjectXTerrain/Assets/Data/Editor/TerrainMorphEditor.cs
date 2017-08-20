using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainMorph))]
public class TerrainMorphEditor : Editor
{
    TerrainMorph terrain;

    void OnEnable()
    {
        terrain = (TerrainMorph)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (!terrain.WasInitialized)
        {
            if (GUILayout.Button("Initialize"))
            {
                terrain.Initialize();
            }
        }
    }
}
