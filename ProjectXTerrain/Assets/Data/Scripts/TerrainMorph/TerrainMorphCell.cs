using TerrainMorphSpace;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainMorphCell : MonoBehaviour
{
    public string Name = "TerrainMorphCell";

    [Header("DON'T TOUCH!!! Only get")]
    [Range(0.01f, 1f)]
    [Tooltip("The number of vertices along the length and width of the cell")]
    public float QuadSize = 0.25f;

    [Range(2, 500)]
    [Tooltip("The size of the sides of the square formed by the vertices" +
        "(or the distance between adjacent vertices without diagonal)")]
    public int VerticesCount = 80;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Transform thisTransform;

    public void Start ()
    {
        InitializeComponents();
    }

    public void Initialize(int id, Vector3 position, 
        float quadSize = 0, int verticesCount = 0)
    {
        var correctId = GetCorrectId(id);

        InitializeComponents();

        if (quadSize != 0)
        {
            QuadSize = quadSize;
        }

        if (verticesCount != 0)
        {
            VerticesCount = verticesCount;
        }

        meshFilter.mesh = TerrainMorphService.CreateCellMesh(
            Name + "Mesh" + correctId, 
            VerticesCount, 
            QuadSize);

        meshRenderer.materials = new Material[1];
        meshRenderer.materials[0] = new Material(Shader.Find("Standard"));
        thisTransform.position = position;
        Name = Name + correctId;
        name = Name;
    }

    private void InitializeComponents()
    {
        if (!meshRenderer)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (!meshFilter)
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        if (!thisTransform)
        {
            thisTransform = GetComponent<Transform>();
        }
    }

    private string GetCorrectId(int id)
    {
        if (id < 10)
        {
            return "00" + id;
        }

        if (id < 100)
        {
            return "0" + id;
        }

        return id.ToString();
    }
}
