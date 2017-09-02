using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainMorphCell))]
public class TerrainMorphCellEditor : Editor
{
    TerrainMorphCell terrainCell;

    void OnEnable()
    {
        terrainCell = (TerrainMorphCell)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Name: " + terrainCell.Name);
        GUILayout.Label("DefaultTexture: " + terrainCell.VerticesCount);
        GUILayout.Label("QuadSize: " + terrainCell.QuadSize);
        DrawDefaultInspector();
    }
}