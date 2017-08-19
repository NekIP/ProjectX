using System;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainMorphSpace
{
    public static class TerrainMorphService
    {
        /// <summary>
        /// Create a new cell
        /// </summary>
        /// <param name="verticesCount">The number of vertices along the length and width of the cell</param>
        /// <param name="quadSize">
        ///     The size of the sides of the square formed by the 
        ///     vertices(or the distance between adjacent vertices without diagonal)
        /// </param>
        /// <param name="name">Common name for this mesh</param>
        public static Mesh CreateCellMesh(string name, int verticesCount, float quadSize)
        {
            var result = new Mesh();
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var colors = new List<Color>();

            for (var i = 0; i < verticesCount; i++)
            {
                for (var j = 0; j < verticesCount; j++)
                {
                    vertices.Add(new Vector3(i * quadSize, 0, j * quadSize));
                    normals.Add(Vector3.Cross(Vector3.forward, Vector3.right).normalized);
                    colors.Add(Color.white);
                }
            }

            result.name = name + "Mesh";
            result.SetVertices(vertices);
            result.SetColors(colors);
            result.SetNormals(normals);
            result.SetTriangles(GetTriangles(vertices, verticesCount, quadSize), 0);
            result.SetUVs(0, GetUvs(verticesCount));

            return result;
        }

        /// <summary>
        /// Generates an array of triangles based on vertex
        /// </summary>
        /// <param name="verticesCount">The number of vertices along the length and width of the cell</param>
        /// <param name="quadSize">
        ///     The size of the sides of the square formed by the 
        ///     vertices(or the distance between adjacent vertices without diagonal)
        /// </param>
        public static int[] GetTriangles(IList<Vector3> vertices, 
            int verticesCount, float quadSize)
        {
            var pointTriangleCount = (int)Mathf.Pow(verticesCount - 1, 2) * 6;
            var pointTriangleOneCount = (verticesCount - 1) * 6;
            var triangles = new int[pointTriangleCount];

            for (var i = 0; i < verticesCount - 1; i++)
            {
                for (var j = 0; j < verticesCount - 1; j++)
                {
                    var ind = i * pointTriangleOneCount + j * 6;
                    triangles[ind] = GetIndexByCoord(vertices,
                        new Vector3(i * quadSize, 0, j * quadSize));
                    triangles[ind + 1] = GetIndexByCoord(vertices,
                        new Vector3(i * quadSize, 0, (j + 1) * quadSize));
                    triangles[ind + 2] = GetIndexByCoord(vertices,
                        new Vector3((i + 1) * quadSize, 0, j * quadSize));
                    triangles[ind + 3] = GetIndexByCoord(vertices,
                        new Vector3(i * quadSize, 0, (j + 1) * quadSize));
                    triangles[ind + 4] = GetIndexByCoord(vertices,
                        new Vector3((i + 1) * quadSize, 0, (j + 1) * quadSize));
                    triangles[ind + 5] = GetIndexByCoord(vertices,
                        new Vector3((i + 1) * quadSize, 0, j * quadSize));
                }
            }

            return triangles;
        }

        /// <summary>
        /// Generates an array of evenly distributing the uv vertices in the uv plane
        /// </summary>
        /// <param name="verticesCount">The number of vertices along the length and width of the cell</param>
        public static List<Vector2> GetUvs(int verticesCount)
        {
            var result = new List<Vector2>();
            for (var i = 0; i < verticesCount; i++)
            {
                for (var j = 0; j < verticesCount; j++)
                {
                    var uv = new Vector2(
                        (float)Math.Round(i / (float)(verticesCount - 1), 2),
                        (float)Math.Round(j / (float)(verticesCount - 1), 2));

                    result.Add(uv);
                }
            }

            return result;
        }

        /// <summary>
        /// Shows the polygons of the model. Works only in methods of OnGUI, OnDrawGizmos and OnDrawGizmosSelected
        /// </summary>
        /// <param name="meshTransform">Need for transform point from local to global </param>
        public static void ShowStructureMesh(Transform meshTransform, Mesh mesh)
        {
            if (mesh.triangles != null && mesh.triangles.Length > 0)
            {
                var defaultColor = Gizmos.color;
                Gizmos.color = Color.green;
                for (var i = 0; i < mesh.triangles.Length; i += 3)
                {
                    Gizmos.DrawLine(
                        meshTransform.TransformPoint(mesh.vertices[mesh.triangles[i]]),
                        meshTransform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]));
                    Gizmos.DrawLine(
                        meshTransform.TransformPoint(mesh.vertices[mesh.triangles[i + 1]]),
                        meshTransform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]));
                    Gizmos.DrawLine(
                        meshTransform.TransformPoint(mesh.vertices[mesh.triangles[i + 2]]),
                        meshTransform.TransformPoint(mesh.vertices[mesh.triangles[i]]));
                }

                Gizmos.color = defaultColor;
            }
        }

        private static int GetIndexByCoord(IList<Vector3> items, Vector3 searchItem)
        {
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i] == searchItem)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
