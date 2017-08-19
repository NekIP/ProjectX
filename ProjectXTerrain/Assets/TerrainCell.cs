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

    public Mesh GetPlain(int size) {
        var plain = new Mesh();
        var countTriangle = (int)Mathf.Pow(size - 1, 2) * 6;

        var vertices = new List<Vector3>();
        var triangles = new int[countTriangle];
        var normals = new List<Vector3>();
        var colors = new List<Color>();

        for (var i = 0; i < Size; i++) {
            for (var j = 0; j < Size; j++) {
                vertices.Add(new Vector3(i, 0, j));
                normals.Add(new Vector3(i, Vector3.up.y, j));
                colors.Add(Color.white);
            }
        }

        for (var i = 0; i < Size - 1; i++) {
            for (var j = 0; j < Size - 1; j++) {
                var ind = 6 * (i * Size + j);
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

    private int GetIndexByCoord(IList<Vector3> items, Vector3 searchItem) {
        for (var i = 0; i < items.Count; i++) {
            if (items[i] == searchItem) {
                return i;
            }
        }

        return -1;
    }
}
