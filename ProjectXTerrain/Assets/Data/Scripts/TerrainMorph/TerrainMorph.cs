using System.Collections.Generic;
using UnityEngine;

public class TerrainMorph : MonoBehaviour
{
    [Header("Terrain settings")]
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

    Transform thisTransform;

	void Start ()
    {
        thisTransform = GetComponent<Transform>();
        Initialize();
    }
	
	void Update ()
    {
		
	}

    public void Initialize()
    {
        if (TryDownloadData())
        {
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
                    thisTransform.position + new Vector3(i * cellSize, 0, j * cellSize),
                    QuadSize,
                    VerticesCount);

                Cells.Add(cellComponent);

                cellObj.transform.parent = thisTransform;
            }
        }
    }

    public bool TryDownloadData()
    {
        return false;
    }

    public void SaveData()
    {

    }
}
