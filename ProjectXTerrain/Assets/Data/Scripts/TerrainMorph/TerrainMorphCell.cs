using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TerrainMorphSpace;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainMorphCell : MonoBehaviour {
    [Range(0.01f, 1f)]
    [Tooltip("The number of vertices along the length and width of the cell")]
    public float QuadSize = 0.25f;

    [Range(2, 500)]
    [Tooltip("The size of the sides of the square formed by the vertices" +
        "(or the distance between adjacent vertices without diagonal)")]
    public int VerticesCount = 80;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;
    Transform thisTransform;

    void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        thisTransform = GetComponent<Transform>();
        mesh = TerrainMorphService.CreateCellMesh(VerticesCount, QuadSize);
        meshFilter.mesh = mesh;
    }
	
	void Update () {
	}

    void OnDrawGizmosSelected() {
        /*if (meshFilter != null && meshFilter.mesh != null) {
            DebugMesh(meshFilter.mesh);
        }*/
    }

}
