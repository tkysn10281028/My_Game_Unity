using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase mapTile;
    [SerializeField] private GameObject mapObject;
    [SerializeField] private Transform mapParent;
    [SerializeField] private int gridWidth = 3;
    [SerializeField] private int gridHeight = 3;
    void Start()
    {
        DrawMap();
        var data = new List<MapObject> { new(0, 0, 1, "player", true) };
        DrawMapObject(data);
    }

    private void DrawMap()
    {
        var xAdjustment = 6;
        var yAdjustment = -5;
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                var pos = new Vector3Int(x + xAdjustment, y + yAdjustment, 0);
                tilemap.SetTile(pos, mapTile);
            }
        }
    }
    private void DrawMapObject(List<MapObject> data)
    {
        var xAdjustment = 8;
        var yAdjustment = -3;
        foreach (var item in data)
        {
            Vector3 basePos = tilemap.CellToWorld(new Vector3Int(item.x + xAdjustment, item.y + yAdjustment));
            switch (item.type)
            {
                case "player":
                    Instantiate(mapObject, basePos - new Vector3(0.5f, 0.5f), Quaternion.identity, mapParent);
                    break;
                case "lock":
                    Instantiate(mapObject, basePos - new Vector3(0.5f, 0.5f), Quaternion.identity, mapParent);
                    break;
                case "property":
                    Instantiate(mapObject, basePos - new Vector3(0.5f, 0.5f), Quaternion.identity, mapParent);
                    break;
            }

        }
    }
}
public class MapObject
{
    public int x;
    public int y;
    public int color;
    public string type;
    public bool isXDirection;

    public MapObject(int x, int y, int color, string type, bool isXDirection = false)
    {
        this.x = x;
        this.y = y;
        this.color = color;
        this.type = type;
        this.isXDirection = isXDirection;

    }
}
