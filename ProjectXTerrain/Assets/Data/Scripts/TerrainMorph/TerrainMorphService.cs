using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TerrainMorphSpace
{
    public static class TerrainMorphService
    {
        public static readonly string DefaultPath = "/Data/Resources/Terrains/";

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

        public static void SaveTerrain(TerrainMorphData terrain)
        {
            CreateDirectoryIfNotExist(DefaultPath + terrain.Name);
            var text = JsonUtility.ToJson(terrain);
            using (var fs = new StreamWriter(File.Open
                (Application.dataPath + DefaultPath + terrain.Name + "/" + terrain.Name + ".json", FileMode.OpenOrCreate)))
            {
                fs.Write(text);
            }
        }

        public static void DeleteTerrain(string terrainName)
        {
            var path = Application.dataPath + DefaultPath + terrainName;
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path).ToList();
                files.ForEach(x => File.Delete(x));
                Directory.Delete(path);
            }
        }

        public static TerrainMorphData LoadTerrain(string terrainName)
        {
            CreateDirectoryIfNotExist(DefaultPath + terrainName);
            using (var fs = new StreamReader(File.Open
                (Application.dataPath + DefaultPath + terrainName + "/" + terrainName + ".json", FileMode.Open)))
            {
                var text = fs.ReadToEnd();
                var result = JsonUtility.FromJson<TerrainMorphData>(text);
                return result;
            }
        }

        public static TerrainMorphData LoadTerrainFromPath(string terrainPath)
        {
            CreateDirectoryIfNotExist(terrainPath);
            using (var fs = new StreamReader(File.Open
                (Application.dataPath + terrainPath, FileMode.Open)))
            {
                var text = fs.ReadToEnd();
                var result = JsonUtility.FromJson<TerrainMorphData>(text);
                return result;
            }
        }

        public static Texture2D CreateTexture(string terrainName, string cellName, Texture2D copy)
        {
            CreateDirectoryIfNotExist(DefaultPath + terrainName);
            AssetDatabase.CreateAsset(copy, "Assets" + DefaultPath + terrainName + "/" + cellName + "Texture.png");
            return copy;
        }

        public static Texture2D GetTexture(string terrainName, string cellName)
        {
            CreateDirectoryIfNotExist(DefaultPath + terrainName);
            return AssetDatabase.LoadAssetAtPath<Texture2D>("Assets" + DefaultPath + terrainName + "/" + cellName + "Texture.png");
        }

        public static bool IsSaved(string terrainName)
        {
            return File.Exists(Application.dataPath + DefaultPath + terrainName + ".json");
        }

        private static void CreateDirectoryIfNotExist(string directory)
        {
            if (!Directory.Exists(Application.dataPath + directory))
            {
                Directory.CreateDirectory(Application.dataPath + directory);
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
