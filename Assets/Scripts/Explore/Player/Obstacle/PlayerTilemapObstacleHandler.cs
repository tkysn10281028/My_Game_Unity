using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTilemapObstacleHandler : MonoBehaviour, IPlayerObstacleHandler
{
    [SerializeField] private Tilemap tilemap;
    private int[,] mapData;
    [SerializeField] private MonoBehaviour processorComponent;
    private TilemapProcessor processor;
    void Awake()
    {
        processor = processorComponent as TilemapProcessor;
    }

    public void Init(int[,] mapData)
    {
        Debug.Log("aaa");
        this.mapData = mapData;
    }

    public async Task<Vector3> CheckObstacle(Vector3 origin, Vector3 target)
    {
        // 処理内容の判定のため移動先のセル座標を取得
        Vector3Int cellPos = tilemap.WorldToCell(target);
        int cols = mapData.GetLength(1);
        int rows = mapData.GetLength(0);
        int x = cellPos.x + cols / 2;
        int y = -(cellPos.y - (rows / 2 - 1));
        if (x < 0 || x >= cols || y < 0 || y >= rows) return origin;
        int cellValue = mapData[y, x];
        return await processor.ProcessAndReturnPosition(origin, target, cellValue);
    }
}