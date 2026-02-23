using UnityEngine;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance;
    // Based on Fast Hydraulic Erosion Simulation and Visualization on GPU - https://inria.hal.science/inria-00402079/document

    public Grid gridComponent;

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GridData.Instance.grid[1, 1].d = 1f;
        GridRenderer.Instance.RenderCell(GridData.Instance.grid[1, 1]);
        GridData.Instance.grid[2, 2].d = 0.5f;
        GridRenderer.Instance.RenderCell(GridData.Instance.grid[2, 2]);
    }

    // Update is called once per frame
    void Update()
    {
    }


    void WaterIncrement()
    {

    }

    void FlowSimulation()
    {

    }

    void RenderCell()
    {

    }

    void RenderGrid()
    {

    }

}
