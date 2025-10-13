using System.Threading.Tasks;
using Boot;
using UnityEngine;
using UnityEngine.Tilemaps;
using UniRx;
using System;
using System.Linq;

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

    public Vector3 ProcessAndReturnPosition(Vector3 origin, Vector3 target, int cellValue)
    {
        switch (cellValue)
        {
            case 1:
                return origin;
            case 2:
                Action showChoicesAction = () =>
                    {
                        DialogSystem.ShowWithChoices(
                            () => DialogSystem.ShowAsync("行動を終了します."),
                            "行動を選択してください:",
                            new[] { "通過", "鍵", "ウイルス" },
                            index =>
                            {
                                switch (index)
                                {
                                    case 0:
                                        Debug.Log("通過");
                                        break;
                                    case 1:
                                        Debug.Log("鍵");
                                        RemoveAndRedraw("lock");
                                        break;
                                    case 2:
                                        Debug.Log("ウイルス");
                                        RemoveAndRedraw("virus");
                                        break;
                                    default:
                                        break;
                                }
                            }
                        );
                    };
                DialogSystem.ShowAsync(showChoicesAction, "次のエリアに行く前に行動を選択できます.");
                return origin;
            case 3:
                return origin;
            case 0:
                return target;
            default:
                return target;
        }
    }
    private void RemoveAndRedraw(string type)
    {
        var list = GameManager.Instance.statusObjectList;
        var target = list.FirstOrDefault(obj => obj.type == type);
        if (target != null)
        {
            list.Remove(target);
        }
        var drawer = FindFirstObjectByType<StatusDrawer>();
        if (drawer != null)
        {
            drawer.DrawStatusObject();
        }
    }
}