using TerrainMorphSpace;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
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

    public Texture2D DefaultTexture;
    public Shader DefaultShader;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Transform thisTransform;

    public void Start ()
    {
        InitializeComponents();
    }

    public void Initialize(int id, Vector3 position, 
        float quadSize, int verticesCount, 
        Texture2D defaultTexture, Shader defaultShader)
    {
        var correctId = GetCorrectId(id);

        InitializeComponents();
        
        QuadSize = quadSize;
        VerticesCount = verticesCount;
        DefaultTexture = defaultTexture;
        DefaultShader = defaultShader;

        meshFilter.sharedMesh = TerrainMorphService.CreateCellMesh(
            Name + "Mesh" + correctId, 
            VerticesCount, 
            QuadSize);

        meshRenderer.materials = new Material[1];

        if (DefaultShader != null)
        {
            meshRenderer.materials[0] = new Material(DefaultShader);
            if (DefaultTexture != null)
            {
                meshRenderer.materials[0].SetTexture("_MainTex", DefaultTexture);
            }
        }

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
