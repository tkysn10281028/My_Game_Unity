using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawTileMapHandler : MonoBehaviour, IDrawFieldHandler
{
    [SerializeField] private Tilemap tilemap;
    private int[,] mapData;
    [SerializeField] private MonoBehaviour obstacleHandlerComponent;
    private PlayerTilemapObstacleHandler obstacleHandler;
    [SerializeField] private MonoBehaviour processorComponent;
    private TilemapProcessor processor;
    void Awake()
    {
        processor = processorComponent as TilemapProcessor;
        obstacleHandler = obstacleHandlerComponent as PlayerTilemapObstacleHandler;
    }
    public void Draw()
    {
        // TODO:ここでリモートからファイル名の指示を受けて読み込みを行う
        LoadFieldFromCSV("map.csv");
        DrawField();
        obstacleHandler.Init(mapData);
    }

    private void LoadFieldFromCSV(string fileName)
    {
        // TODO:ここはS3のパスに置き換える。もしくはBootScene上でファイルを全てS3からダウンロードするなら変更なし
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogError("CSV file not found : " + fileName);
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
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                processor.SetTile(mapData[y, x], new Vector3Int(x + xAdjust, -y + yAdjust, 0), tilemap);
            }
        }
    }

    public int[,] GetMapData()
    {
        return mapData;
    }
}
