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
        public TerrainMorphTransform Transform { get; set; }
        public List<TerrainMorphCellData> Cells { get; set; }

        public static TerrainMorphData Map(TerrainMorph item)
        {
            return new TerrainMorphData
            {
                Name = item.name,
                QuadSize = item.QuadSize,
                VerticesCount = item.VerticesCount,
                DefaultTexture = item.DefaultTexture,
                DefaultShader = item.DefaultShader,
                Cells = item.Cells.Select(TerrainMorphCellData.Map).ToList()
            };
        }

        public static TerrainMorph Map(TerrainMorphData item)
        {
            var terrainObj = new GameObject(item.Name, typeof(TerrainMorph));

            var terrainComponent = terrainObj.GetComponent<TerrainMorph>();
            terrainComponent.name = item.Name;
            terrainComponent.QuadSize = item.QuadSize;
            terrainComponent.VerticesCount = item.VerticesCount;
            terrainComponent.DefaultTexture = item.DefaultTexture;
            terrainComponent.DefaultShader = item.DefaultShader;
            terrainComponent.Cells = item.Cells.Select(TerrainMorphCellData.Map).ToList();

            return terrainComponent;
        }
    }

    public class TerrainMorphCellData
    {
        public string Name { get; set; }
        public float QuadSize { get; set; }
        public int VerticesCount { get; set; }
        public Texture2D DefaultTexture { get; set; }
        public Shader DefaultShader { get; set; }
        public TerrainMorphTransform Transform { get; set; }
        public TerrainMorphCellMeshData Mesh { get; set; }

        public static TerrainMorphCellData Map(TerrainMorphCell item)
        {
            return new TerrainMorphCellData
            {
                Name = item.Name,
                QuadSize = item.QuadSize,
                VerticesCount = item.VerticesCount,
                DefaultTexture = item.DefaultTexture,
                DefaultShader = item.DefaultShader,
                Mesh = TerrainMorphCellMeshData.Map(item.Mesh),
                Transform = TerrainMorphTransform.Map(item.transform)
            };
        }

        public static TerrainMorphCell Map(TerrainMorphCellData item)
        {
            var cellObj = new GameObject(item.Name, typeof(TerrainMorphCell));
            var cellComponent = cellObj.GetComponent<TerrainMorphCell>();

            cellComponent.Name = item.Name;
            cellComponent.QuadSize = item.QuadSize;
            cellComponent.VerticesCount = item.VerticesCount;
            cellComponent.DefaultTexture = item.DefaultTexture;
            cellComponent.DefaultShader = item.DefaultShader;
            cellComponent.Mesh = TerrainMorphCellMeshData.Map(item.Mesh);
            cellComponent.transform.position = item.Transform.Position;
            cellComponent.transform.rotation = item.Transform.Rotation;
            cellComponent.transform.localScale = item.Transform.Scale;

            return cellComponent;
        }
    }

    public class TerrainMorphCellMeshData
    {
        public string Name { get; set; }
        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }
        public Vector2[] Uvs { get; set; }
        public Color[] Colors { get; set; }
        public int[] Triangles { get; set; }

        public static TerrainMorphCellMeshData Map(Mesh item)
        {
            return new TerrainMorphCellMeshData
            {
                Name = item.name,
                Vertices = item.vertices,
                Normals = item.normals,
                Uvs = item.uv,
                Colors = item.colors,
                Triangles = item.triangles
            };
        }

        public static Mesh Map(TerrainMorphCellMeshData item)
        {
            return new Mesh
            {
                name = item.Name,
                vertices = item.Vertices,
                normals = item.Normals,
                uv = item.Uvs,
                colors = item.Colors,
                triangles = item.Triangles
            };
        }
    }

    public class TerrainMorphTransform
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public static TerrainMorphTransform Map(Transform item)
        {
            return new TerrainMorphTransform
            {
                Position = item.position,
                Rotation = item.rotation,
                Scale = item.localScale
            };
        }
    }
}
