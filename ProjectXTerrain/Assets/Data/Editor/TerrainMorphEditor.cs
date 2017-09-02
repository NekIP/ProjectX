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
        GUILayout.Label("Name: " + terrain.Name);
        DrawDefaultInspector();
        if (!terrain.WasInitialized)
        {
            if (GUILayout.Button("Initialize"))
            {
                terrain.Initialize();
            }
        }
        else
        {
            if (GUILayout.Button("Save"))
            {
                terrain.SaveData();
            }
            if (GUILayout.Button("Remove"))
            {
                terrain.Delete();
            }
            if (GUILayout.Button("Remove(with saved)"))
            {
                terrain.Delete(true);
            }
        }
    }
}
