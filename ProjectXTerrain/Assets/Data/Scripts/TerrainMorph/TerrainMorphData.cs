using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TerrainMorphSpace
{
    [Serializable]
    public class TerrainMorphData
    {
        public string Name;
        public float QuadSize;
        public int VerticesCount;
        public Texture2D DefaultTexture;
        public Shader DefaultShader;
        public TerrainMorphTransform Transform;
        public int CellCountInOneSideDefault;
        public List<TerrainMorphCellData> Cells;

        public static TerrainMorphData Map(TerrainMorph item)
        {
            return new TerrainMorphData
            {
                Name = item.Name,
                QuadSize = item.QuadSize,
                VerticesCount = item.VerticesCount,
                DefaultTexture = item.DefaultTexture,
                DefaultShader = item.DefaultShader,
                CellCountInOneSideDefault = item.CellCountInOneSideDefault,
                Transform = TerrainMorphTransform.Map(item.transform),
                Cells = item.Cells.Select(TerrainMorphCellData.Map).ToList()
            };
        }
    }

    [Serializable]
    public class TerrainMorphCellData
    {
        public string Name;
        public float QuadSize;
        public int VerticesCount;
        public Texture2D DefaultTexture;
        public Shader DefaultShader;
        public TerrainMorphTransform Transform;
        public TerrainMorphCellMeshData Mesh;

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

        public static TerrainMorphCell Map(TerrainMorphCellData item, string terrainName)
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
            cellComponent.MeshRenderer.materials = new Material[1];
            cellComponent.MeshRenderer.materials[0] = new Material(item.DefaultShader);
            cellComponent.MeshRenderer.materials[0].SetTexture(cellComponent.TextureNameInShader, 
                TerrainMorphService.GetTexture(terrainName, item.Name));

            return cellComponent;
        }
    }

    [Serializable]
    public class TerrainMorphCellMeshData
    {
        public string Name;
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public Vector2[] Uvs;
        public Color[] Colors;
        public int[] Triangles;

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

    [Serializable]
    public class TerrainMorphTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

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
