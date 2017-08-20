using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TerrainMorphSpace
{
    public class TerrainMorphData
    {
        public string Name { get; set; }
        public float QuadSize { get; set; }
        public int VerticesCount { get; set; }
        public Texture2D DefaultTexture { get; set; }
        public Shader DefaultShader { get; set; }
        public TerrainMorphCellData Cells { get; set; }
    }

    public class TerrainMorphCellData
    {
        public string Name { get; set; }
        public float QuadSize { get; set; }
        public int VerticesCount { get; set; }
        public Texture2D DefaultTexture { get; set; }
        public Shader DefaultShader { get; set; }
        public TerrainMorphCellMeshData Mesh { get; set; }
    }

    public class TerrainMorphCellMeshData
    {
        public string Name { get; set; }
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector3> Uvs { get; set; }
        public List<Color> Colors { get; set; }
        public List<int> Triangles { get; set; }
    }
}
