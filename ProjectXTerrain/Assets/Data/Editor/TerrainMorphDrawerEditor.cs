using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class TerrainMorphDrawerEditor
{
    public static void DrawCircleOnMouse(float radius, int terrainLayer = 31)
    {
        var mousePosition = Event.current.mousePosition;
        var ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Gizmos.DrawSphere(hit.point, radius);
            SceneView.currentDrawingSceneView.OnSelectionChange();
            Debug.DrawLine(hit.point, new Vector3(hit.point.x, 1, hit.point.z));
        }
    }
}

