using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawTileMapHandler : MonoBehaviour, IDrawFieldHandler
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase outerWallTile;
    [SerializeField] private TileBase innerWallTile;
    [SerializeField] private TileBase obstacleTile;
    private int[,] mapData;

    public void Draw()
    {
        LoadFieldFromCSV("map.csv");
        DrawField();
        var obstacleHandler = FindFirstObjectByType<PlayerTilemapObstacleHandler>();
        obstacleHandler.Init(mapData);
    }

    private void LoadFieldFromCSV(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogError("CSV file not found at " + path);
            return;
        }
        string[] lines = File.ReadAllLines(path);
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length;

        mapData = new int[rows, cols];

        for (int y = 0; y < rows; y++)
        {
            string[] values = lines[y].Split(',');
            for (int x = 0; x < cols; x++)
            {
                if (int.TryParse(values[x], out int val))
                {
                    mapData[y, x] = val;
                }
                else
                {
                    Debug.LogWarning($"Invalid value at ({y},{x}) in CSV.");
                    mapData[y, x] = 0;
                }
            }
        }
    }
    private void DrawField()
    {
        int rows = mapData.GetLength(0);
        int cols = mapData.GetLength(1);
        int xAdjust = -cols / 2;
        int yAdjust = rows / 2 - 1;
        var processor = new TilemapProcessor();
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                processor.setTile(mapData[y, x], new Vector3Int(x + xAdjust, -y + yAdjust, 0), tilemap);
            }
        }
    }

    public int[,] GetMapData()
    {
        return mapData;
    }
}
