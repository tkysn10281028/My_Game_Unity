using System.Threading.Tasks;
using Boot;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapProcessor : MonoBehaviour
{
    [SerializeField] private TileBase outerWallTile;
    [SerializeField] private TileBase innerWallTile;
    [SerializeField] private TileBase floorTile;
    public void SetTile(int cellValue, Vector3Int cellPos, Tilemap tilemap)
    {
        switch (cellValue)
        {
            case 1:
                tilemap.SetTile(cellPos, outerWallTile);
                break;
            case 2:
                tilemap.SetTile(cellPos, innerWallTile);
                break;
            case 3:
                tilemap.SetTile(cellPos, innerWallTile);
                break;
            case 0:
                tilemap.SetTile(cellPos, floorTile);
                break;
        }
    }

    public async Task<Vector3> ProcessAndReturnPosition(Vector3 origin, Vector3 target, int cellValue)
    {
        switch (cellValue)
        {
            case 1:
                return origin;
            case 2:
                GameManager.Instance.LockPlayer();
                DialogSystem.Show("テスト");
                return origin;
            case 3:
                // TODO: ここでダイアログを出す
                GameManager.Instance.LockPlayer();
                DialogSystem.Show("テスト");
                return origin;
            case 0:
                return target;
            default:
                return target;
        }
    }
}