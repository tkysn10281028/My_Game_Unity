using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTilemapObstacleHandler : MonoBehaviour, IPlayerObstacleHandler
{
    [SerializeField] private Tilemap tilemap;
    private int[,] mapData;

    public void Init(int[,] mapData)
    {
        this.mapData = mapData;
    }

    public Vector3 CheckObstacle(Vector3 origin, Vector3 target)
    {
        // 処理内容の判定のため移動先のセル座標を取得
        Vector3Int cellPos = tilemap.WorldToCell(target);
        int cols = mapData.GetLength(1);
        int rows = mapData.GetLength(0);
        int x = cellPos.x + cols / 2;
        int y = -(cellPos.y - (rows / 2 - 1));
        if (x < 0 || x >= cols || y < 0 || y >= rows) return origin;

        int cellValue = mapData[y, x];
        switch (cellValue)
        {
            case 1:
                return origin;
            case 2:
                Debug.Log("障害物に当たった！追加処理");
                return origin;
            case 0:
            default:
                return target;
        }
    }
}