using UnityEngine;
[DefaultExecutionOrder(-100)]
public class GridData : MonoBehaviour
{
    public static GridData Instance;

    [Header("Grid Settings")]
    public Vector2Int gridSize;
    public float cellSize = 1f;

    [Header("Grid Content")]
    public Cell[,] grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        Instance = this;
        InitGrid();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitGrid()
    {
        Debug.Log($"Starting GridData's Init Grid with size {gridSize.x},{gridSize.y}");
        grid = new Cell[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x - 1; x++)
        {
            for (int y = 0; y < gridSize.y - 1; y++)
            {
                grid[x, y] = new Cell(x, y);
            }
        }
        Debug.Log("GridData's grid initialized!");
    }
}
