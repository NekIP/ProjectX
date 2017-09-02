using TerrainMorphSpace;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class TerrainMorphCell : MonoBehaviour
{
    [HideInInspector]
    public string Name = "TerrainMorphCell";

    [Header("DON'T TOUCH!!! Only get")]
    [Range(0.01f, 1f)]
    [Tooltip("The number of vertices along the length and width of the cell")]
    [HideInInspector]
    public float QuadSize = 0.25f;

    [Range(2, 500)]
    [Tooltip("The size of the sides of the square formed by the vertices" +
        "(or the distance between adjacent vertices without diagonal)")]
    [HideInInspector]
    public int VerticesCount = 80;

    public Texture2D DefaultTexture;
    public Shader DefaultShader;
    [HideInInspector]
    public string TextureNameInShader = "_MainTex";

    public Mesh Mesh
    {
        get
        {
            if (!meshFilter)
            {
                meshFilter = GetComponent<MeshFilter>();
            }

            return meshFilter.sharedMesh;
        }
        set
        {
            if (!meshFilter)
            {
                meshFilter = GetComponent<MeshFilter>();
            }

            meshFilter.sharedMesh = value;
        }
    }

    public MeshRenderer MeshRenderer
    {
        get
        {
            if (!meshRenderer)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }

            return meshRenderer;
        }
    }

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Transform thisTransform;

    public void Start ()
    {
        InitializeComponents();
    }

    public void Initialize(int id, string terrainName, Vector3 position, 
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

        Name = Name + correctId;
        name = Name;

        meshRenderer.materials = new Material[1];

        if (DefaultShader != null)
        {
            meshRenderer.materials[0] = new Material(DefaultShader);
            if (DefaultTexture != null)
            {
                meshRenderer.materials[0].SetTexture(TextureNameInShader, 
                    TerrainMorphService.CreateTexture(terrainName, Name, Instantiate(DefaultTexture)));
            }
        }

        thisTransform.position = position;
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
