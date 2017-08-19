using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainCell : MonoBehaviour {
    [Range(0.01f, 1f)]
    public float QuadSize = 0.5f;
    [Range(2, 500)]
    public int QuadCount = 4;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;
    Transform thisTransform;

    void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        thisTransform = GetComponent<Transform>();
        mesh = CreateCell(QuadCount);
        meshFilter.mesh = mesh;
    }
	
	void Update () {
	}

    void OnDrawGizmos() {
        /*if (meshFilter != null && meshFilter.mesh != null) {
            DebugMesh(meshFilter.mesh);
        }*/
    }

    public Mesh CreateCell(int size) {
        var result = new Mesh();
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var colors = new List<Color>();

        for (var i = 0; i < size; i++) {
            for (var j = 0; j < size; j++) {
                vertices.Add(new Vector3(i * QuadSize, 0, j * QuadSize));
                normals.Add(Vector3.Cross(Vector3.forward, Vector3.right).normalized);
                colors.Add(Color.white);
            }
        }

        result.name = "plain";
        result.SetVertices(vertices);
        result.SetColors(colors);
        result.SetNormals(normals);
        result.SetTriangles(GetTriangles(vertices, size), 0);
        result.SetUVs(0, GetUvs(size));

        return result;
    }

    public int[] GetTriangles(IList<Vector3> vertices, int size) {
        var pointTriangleCount = (int)Mathf.Pow(size - 1, 2) * 6;
        var pointTriangleOneCount = (size - 1) * 6;
        var triangles = new int[pointTriangleCount];

        for (var i = 0; i < size - 1; i++) {
            for (var j = 0; j < size - 1; j++) {
                var ind = i * pointTriangleOneCount + j * 6;
                triangles[ind] = GetIndexByCoord(vertices,
                    new Vector3(i * QuadSize, 0, j * QuadSize));
                triangles[ind + 1] = GetIndexByCoord(vertices,
                    new Vector3(i * QuadSize, 0, (j + 1) * QuadSize));
                triangles[ind + 2] = GetIndexByCoord(vertices,
                    new Vector3((i + 1) * QuadSize, 0, j * QuadSize));
                triangles[ind + 3] = GetIndexByCoord(vertices,
                    new Vector3(i * QuadSize, 0, (j + 1) * QuadSize));
                triangles[ind + 4] = GetIndexByCoord(vertices,
                    new Vector3((i + 1) * QuadSize, 0, (j + 1) * QuadSize));
                triangles[ind + 5] = GetIndexByCoord(vertices,
                    new Vector3((i + 1) * QuadSize, 0, j * QuadSize));
            }
        }

        return triangles;
    }

    public List<Vector2> GetUvs(int size) {
        var result = new List<Vector2>();
        for (var i = 0; i < size; i++) {
            for (var j = 0; j < size; j++) {
                var uv = new Vector2(
                    (float)Math.Round(i / (float)(size - 1), 2),
                    (float)Math.Round(j / (float)(size - 1), 2));

                result.Add(uv);
            }
        }

        return result;
    }

    public Mesh CreateCell(Vector3 origin, Vector3 width, 
        Vector3 length, int size) {
        var cell = new Mesh();
        var count = size * size;
        var combine = new CombineInstance[count];

        var k = 0;
        for (var x = 0; x < size; x++) {
            for (var y = 0; y < size; y++) {
                combine[k].mesh = CreateQuad(origin + width * x + length * y, width, length, size);
                k++;
            }
        }

        var uv = new Vector2[size * size];
        for (var i = 0; i < size; i++) {
            for (var j = 0; j < size; j++) {
                uv[i * size + j] = new Vector2(i / (float)count,
                    j / (float)count);
            }
        }

        cell.name = "Cell";
        cell.CombineMeshes(combine, true, false);
        cell.uv = uv;
        
        return cell;
    }

    public Mesh CreateQuad(Vector3 origin, Vector3 width, Vector3 length, int size) {
        var normal = Vector3.Cross(length, width).normalized;
        var quad = new Mesh {
            vertices = new[] { origin, origin + length, origin + length + width, origin + width },
            normals = new[] { normal, normal, normal, normal },
            uv = new[] { new Vector2(0, 0), new Vector2(0, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0) },
            triangles = new[] { 0, 1, 2, 0, 2, 3 }
        };

        return quad;
    }

    public void DebugMesh(Mesh mesh) {
        if (mesh.triangles != null && mesh.triangles.Length > 0) {
            var defaultColor = Gizmos.color;
            Gizmos.color = Color.green;
            for (var i = 0; i < mesh.triangles.Length; i += 3) {
                Gizmos.DrawLine(
                    thisTransform.TransformPoint(mesh.vertices[mesh.triangles[i]]),
                    thisTransform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]));
                Gizmos.DrawLine(
                    thisTransform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]),
                    thisTransform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]));
                Gizmos.DrawLine(
                    thisTransform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]),
                    thisTransform.TransformPoint(mesh.vertices[mesh.triangles[i]]));
            }
            
            Gizmos.color = defaultColor;
        }
    }

    private int GetIndexByCoord(IList<Vector3> items, Vector3 searchItem) {
        for (var i = 0; i < items.Count; i++) {
            if (items[i] == searchItem) {
                return i;
            }
        }

        return -1;
    }
}
