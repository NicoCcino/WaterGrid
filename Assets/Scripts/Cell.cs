using UnityEngine;

[System.Serializable]
public class Cell
{
    public int x; // posX
    public int y; // posY
    public float b; // terrainHeight
    public float d; // waterHeight
    public float fL; // flowLeft
    public float fR; // flowRight
    public float fT; // flowTop
    public float fB; // flowBottom
    public Vector2 v; // velocityVector

    public Cell(int _x, int _y)
    {
        x = _x;
        y = _y;
        d = 0f;
    }
}
