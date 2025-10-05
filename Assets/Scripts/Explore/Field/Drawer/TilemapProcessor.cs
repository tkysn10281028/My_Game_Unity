using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapProcessor
{
    [SerializeField] private TileBase outerWallTile;
    [SerializeField] private TileBase innerWallTile;
    [SerializeField] private TileBase obstacleTile;
    public void setTile(int cellValue, Vector3Int cellPos, Tilemap tilemap)
    {
        switch (cellValue)
        {
            case 1:
                tilemap.SetTile(cellPos, outerWallTile);
                break;

            case 2:
                tilemap.SetTile(cellPos, innerWallTile);
                break;

            case 0:
                tilemap.SetTile(cellPos, obstacleTile);
                break;
        }
    }
}