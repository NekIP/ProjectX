using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainCell : MonoBehaviour {
    public int Size = 2;

    Mesh mesh;
    MeshFilter meshFilter;

    void Start () {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        mesh = GetPlain(Size);
        meshFilter.mesh = mesh;
    }
	
	void Update () {
	}

    void OnDrawGizmos() {
        if (meshFilter != null && meshFilter.mesh != null) {
            DebugMesh(meshFilter.mesh);
        }
    }

    public Mesh GetPlain(int size) {
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

    public void DebugMesh(Mesh mesh) {
        if (mesh.vertices != null && mesh.vertices.Length > 0) {
            var defaultColor = Gizmos.color;
            Gizmos.color = Color.green;
            foreach (var vertice in mesh.vertices) {
                Gizmos.DrawSphere(vertice, 0.1f);
            }

            Gizmos.color = defaultColor;
        }

        if (mesh.triangles != null && mesh.triangles.Length > 0) {
            var defaultColor = Gizmos.color;
            Gizmos.color = Color.green;
            for (var i = 0; i < mesh.triangles.Length; i += 3) {
                Gizmos.DrawLine(mesh.vertices[mesh.triangles[i]], 
                    mesh.vertices[mesh.triangles[i + 1]]);
                Gizmos.DrawLine(mesh.vertices[mesh.triangles[i + 1]],
                    mesh.vertices[mesh.triangles[i + 2]]);
                Gizmos.DrawLine(mesh.vertices[mesh.triangles[i + 2]], 
                    mesh.vertices[mesh.triangles[i]]);
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
