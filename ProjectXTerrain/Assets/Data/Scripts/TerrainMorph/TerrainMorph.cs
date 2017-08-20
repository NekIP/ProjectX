using System.Collections.Generic;
using System.Linq;
using TerrainMorphSpace;
using UnityEngine;

public class TerrainMorph : MonoBehaviour
{
    [Header("Terrain settings")]
    public string Name;

    [Range(1, 1000)]
    [Tooltip("The initial  number of cells on one side")]
    public int CellCountInOneSideDefault = 3;

    public List<TerrainMorphCell> Cells = new List<TerrainMorphCell>();

    [Header("Cell settings")]
    [Range(0.01f, 1f)]
    [Tooltip("The number of vertices along the length and width of the cell")]
    public float QuadSize = 0.25f;

    [Range(2, 500)]
    [Tooltip("The size of the sides of the square formed by the vertices" +
        "(or the distance between adjacent vertices without diagonal)")]
    public int VerticesCount = 80;

    public Texture2D DefaultTexture;
    public Shader DefaultShader;

    [HideInInspector]
    public bool WasInitialized = false;

    Transform thisTransform;

	void Start ()
    {
        InitializeComponents();
    }
	
    public void Initialize()
    {
        InitializeComponents();

        if (TryDownloadData())
        {
            WasInitialized = true;
            return;
        }

        var cellSize = QuadSize * (VerticesCount - 1);
        for (var i = 0; i < CellCountInOneSideDefault; i++)
        {
            for (var j = 0; j < CellCountInOneSideDefault; j++)
            {
                var cellObj = new GameObject("TerrainMorphCell", typeof(TerrainMorphCell));
                var cellComponent = cellObj.GetComponent<TerrainMorphCell>();
                cellComponent.Initialize(
                    Cells.Count,
                    Name,
                    thisTransform.position + new Vector3(i * cellSize, 0, j * cellSize),
                    QuadSize,
                    VerticesCount,
                    DefaultTexture,
                    DefaultShader);

                Cells.Add(cellComponent);

                cellObj.transform.parent = thisTransform;
            }
        }

        WasInitialized = true;
    }

    public bool TryDownloadData()
    {
        if (TerrainMorphService.IsSaved(Name))
        {
            var data = TerrainMorphService.LoadTerrain(Name);
            CellCountInOneSideDefault = data.CellCountInOneSideDefault;
            Cells = data.Cells.Select(x => TerrainMorphCellData.Map(x, Name)).ToList();
            QuadSize = data.QuadSize;
            VerticesCount = data.VerticesCount;
            DefaultTexture = data.DefaultTexture;
            DefaultShader = data.DefaultShader;
            Cells.ForEach(cell => cell.transform.parent = thisTransform);
            return true;
        }

        return false;
    }

    public void SaveData()
    {
        TerrainMorphService.SaveTerrain(TerrainMorphData.Map(this));
    }

    private void InitializeComponents()
    {
        Name = name;
        if (!thisTransform)
        {
            thisTransform = GetComponent<Transform>();
        }
    }
}
