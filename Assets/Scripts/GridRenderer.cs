using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[DefaultExecutionOrder(-90)]
public class GridRenderer : MonoBehaviour
{
    public static GridRenderer Instance;

    [Header("Grid Settings")]
    public Vector2Int gridSize;
    public float cellSize = 1f;

    [Header("Colors")]
    public Color defaultCellColor = new Color(0, 0, 0, 0);

    private Color[,] cellColors;
    private Color[] meshColors;

    private Mesh mesh;
    private MeshFilter meshFilter;

    // ======================
    void Awake()
    {
        Instance = this;
        Init();
    }

    void OnEnable()
    {
        Instance = this;
        Init();
        RebuildMesh();
    }

    void Start()
    {
        gridSize = GridData.Instance.gridSize;
        // SetCellColor(2, 3, Color.red);
    }

    void Init()
    {
        meshFilter = GetComponent<MeshFilter>();

        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "GridMesh";
            meshFilter.sharedMesh = mesh;
        }

        InitGrid();
    }

    void InitGrid()
    {
        if (cellColors == null ||
            cellColors.GetLength(0) != gridSize.x ||
            cellColors.GetLength(1) != gridSize.y)
        {
            cellColors = new Color[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
                for (int y = 0; y < gridSize.y; y++)
                    cellColors[x, y] = defaultCellColor;
        }
    }

    public void RenderCell(Cell cell)
    {
        if (!IsInside(cell.x, cell.y)) return;

        // couleur
        Color c = new Color(0f, 0f, cell.d);
        SetCellColor(cell.x, cell.y, c);

        // 🔥 update hauteur (fill)
        SetCellHeight(cell.x, cell.y, cell.d);
    }

    public void SetCellColor(int x, int y, Color color)
    {
        if (!IsInside(x, y)) return;

        cellColors[x, y] = color;

        // 🔥 update partiel
        int quadIndex = x * gridSize.y + y;
        int v = quadIndex * 4;

        meshColors[v + 0] = color;
        meshColors[v + 1] = color;
        meshColors[v + 2] = color;
        meshColors[v + 3] = color;

        mesh.colors = meshColors;
    }

    public void SetCellHeight(int x, int y, float d)
    {
        if (!IsInside(x, y)) return;

        // calcul fill (hauteur)
        float x0 = x * cellSize;
        float y0 = y * cellSize;
        float x1 = x0 + cellSize;

        float fill = Mathf.Clamp01(d) * cellSize;
        float yFillTop = y0 + fill;

        // 🔥 update vertices de la cellule
        int quadIndex = x * gridSize.y + y;
        int v = quadIndex * 4;

        Vector3[] vertices = mesh.vertices;

        vertices[v + 0] = new Vector3(x0, y0, 0);
        vertices[v + 1] = new Vector3(x1, y0, 0);
        vertices[v + 2] = new Vector3(x1, yFillTop, 0);
        vertices[v + 3] = new Vector3(x0, yFillTop, 0);

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    bool IsInside(int x, int y)
    {
        return x >= 0 && y >= 0 &&
               x < gridSize.x && y < gridSize.y;
    }

    // ======================
    // 🔥 MESH
    // ======================
    public void RebuildMesh()
    {
        Debug.Log("Starting RebuildMesh");
        InitGrid();

        int quadCount = gridSize.x * gridSize.y;

        Vector3[] vertices = new Vector3[quadCount * 4];
        int[] triangles = new int[quadCount * 6];
        meshColors = new Color[quadCount * 4];

        int v = 0;
        int t = 0;

        for (int x = 0; x < gridSize.x - 1; x++)
            for (int y = 0; y < gridSize.y - 1; y++)
            {
                Cell cell = GridData.Instance.grid[x, y];
                if (cell == null) Debug.Log($"Cell {x},{y} is null");

                float x0 = x * cellSize;
                float y0 = y * cellSize;
                float x1 = x0 + cellSize;

                // ⭐ hauteur remplie (0 → cellSize)
                float fill = Mathf.Clamp01(cell.d) * cellSize;
                float yFillTop = y0 + fill;

                // vertices (quad partiel)
                vertices[v + 0] = new Vector3(x0, y0, 0);
                vertices[v + 1] = new Vector3(x1, y0, 0);
                vertices[v + 2] = new Vector3(x1, yFillTop, 0);
                vertices[v + 3] = new Vector3(x0, yFillTop, 0);

                Color c = cellColors[x, y];

                meshColors[v + 0] = c;
                meshColors[v + 1] = c;
                meshColors[v + 2] = c;
                meshColors[v + 3] = c;

                // triangles
                triangles[t + 0] = v + 0;
                triangles[t + 1] = v + 2;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 0;
                triangles[t + 4] = v + 3;
                triangles[t + 5] = v + 2;

                v += 4;
                t += 6;
            }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = meshColors;
        mesh.RecalculateBounds();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Vector3 origin = transform.position;

        for (int x = 0; x <= gridSize.x; x++)
        {
            Gizmos.DrawLine(
                origin + new Vector3(x * cellSize, 0, 0),
                origin + new Vector3(x * cellSize, gridSize.y * cellSize, 0));
        }

        for (int y = 0; y <= gridSize.y; y++)
        {
            Gizmos.DrawLine(
                origin + new Vector3(0, y * cellSize, 0),
                origin + new Vector3(gridSize.x * cellSize, y * cellSize, 0));
        }
    }
#endif
}