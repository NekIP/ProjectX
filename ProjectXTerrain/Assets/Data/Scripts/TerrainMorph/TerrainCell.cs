using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainCell : MonoBehaviour {
    public int Size = 2;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;
    Transform thisTransform;

    void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        thisTransform = GetComponent<Transform>();
        mesh = CreatePlain(Vector3.zero, Vector3.right, Vector3.forward, Size);
        meshFilter.mesh = mesh;
    }
	
	void Update () {
	}

    void OnDrawGizmos() {
        /*if (meshFilter != null && meshFilter.mesh != null) {
            DebugMesh(meshFilter.mesh);
        }*/
    }

    public Mesh GetPlain01(int size) {
        var plain = new Mesh();

        var countPointTriangleOne = (size - 1) * 6;
        var countPointTriangle = (int)Mathf.Pow(size - 1, 2) * 6;

        var vertices = new List<Vector3>();
        var triangles = new int[countPointTriangle];
        var normals = new List<Vector3>();
        var colors = new List<Color>();

        for (var i = 0; i < size; i++) {
            for (var j = 0; j < size; j++) {
                vertices.Add(new Vector3(i, 0, j));
                normals.Add(new Vector3(i, Vector3.up.y, j));
                colors.Add(Color.white);
            }
        }

        for (var i = 0; i < size - 1; i++) {
            for (var j = 0; j < size - 1; j++) {
                var ind = i * countPointTriangleOne + j * 6;
                triangles[ind] = GetIndexByCoord(vertices, 
                    new Vector3(i, 0, j));
                triangles[ind + 1] = GetIndexByCoord(vertices,
                    new Vector3(i, 0, j + 1));
                triangles[ind + 2] = GetIndexByCoord(vertices,
                    new Vector3(i + 1, 0, j));
                triangles[ind + 3] = GetIndexByCoord(vertices,
                    new Vector3(i, 0, j + 1));
                triangles[ind + 4] = GetIndexByCoord(vertices,
                    new Vector3(i + 1, 0, j + 1));
                triangles[ind + 5] = GetIndexByCoord(vertices,
                    new Vector3(i + 1, 0, j));
            }
        }

        plain.name = "plain";
        plain.SetVertices(vertices);
        plain.SetColors(colors);
        plain.SetNormals(normals);
        plain.SetTriangles(triangles, 0);

        return plain;
    }

    public Mesh CreatePlain(Vector3 origin, Vector3 width, 
        Vector3 length, int size)
    {
        var combine = new CombineInstance[size * size];

        var i = 0;
        for (var x = 0; x < size; x++) {
            for (var y = 0; y < size; y++) {
                combine[i].mesh = Quad(origin + width * x + length * y, width, length);
                i++;
            }
        }

        var plain = new Mesh();
        plain.CombineMeshes(combine, true, false);

        return plain;
    }

    public Mesh Quad(Vector3 origin, Vector3 width, Vector3 length) {
        var normal = Vector3.Cross(length, width).normalized;
        var quad = new Mesh {
            vertices = new[] { origin, origin + length, origin + length + width, origin + width },
            normals = new[] { normal, normal, normal, normal },
            uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) },
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
