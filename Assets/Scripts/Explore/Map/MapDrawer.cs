using System;
using System.Collections.Generic;
using Boot;
using Common.Enum;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase mapTile;
    [SerializeField] private TileBase mapTileYellow;
    [SerializeField] private TileBase mapTileGreen;
    [SerializeField] private GameObject playerYellow;
    [SerializeField] private GameObject playerGreen;
    [SerializeField] private GameObject regist;
    [SerializeField] private GameObject lockYellow;
    [SerializeField] private GameObject lockGreen;
    [SerializeField] private GameObject virus;
    [SerializeField] private Transform mapParent;
    [SerializeField] private int gridWidth = 3;
    [SerializeField] private int gridHeight = 3;
    private Dictionary<Objects, Action<MapObject>> mapObjectActionMap;
    void Awake()
    {
        mapObjectActionMap = new()
        {
           {
                Objects.Player, obj =>
                {
                    Vector3 basePos = tilemap.CellToWorld(new Vector3Int(obj.x + 8, obj.y -3));
                    if(obj.color == 1)
                        Instantiate(playerYellow, basePos - new Vector3(0.5f, 0.5f), Quaternion.identity, mapParent);
                    else
                        Instantiate(playerGreen, basePos - new Vector3(0.5f, 0.5f), Quaternion.identity, mapParent);
                }
           },
           {
                Objects.Resist, obj =>
                {
                    GameObject instance = null;
                    Vector3 basePos = tilemap.CellToWorld(new Vector3Int(obj.x + 8, obj.y -3));
                    instance = Instantiate(regist, basePos - new Vector3(0.5f, 0.5f), Quaternion.identity, mapParent);
                    instance.GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
           },
           {
                Objects.Lock, obj =>
                {
                    GameObject instance = null;
                    Vector3 basePos = tilemap.CellToWorld(new Vector3Int(obj.x + 8, obj.y -3));
                    if(obj.isXDirection)
                        instance = Instantiate(lockYellow, basePos - new Vector3(0, 0.5f), Quaternion.identity, mapParent);
                    else
                        instance = Instantiate(lockGreen, basePos - new Vector3(0.5f, 0), Quaternion.identity, mapParent);
                    instance.GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
           },
           {
                Objects.Ownership, obj =>
                {
                    var xAdjustment = 7;
                    var yAdjustment = -4;
                    var basePos = new Vector3Int(obj.x + xAdjustment, obj.y + yAdjustment, 0);
                    if(obj.color == 1)
                        tilemap.SetTile(basePos,mapTileYellow);
                    else
                        tilemap.SetTile(basePos,mapTileGreen);
                }
           },
           {
                Objects.Virus, obj =>
                {
                    GameObject instance = null;
                    Vector3 basePos = tilemap.CellToWorld(new Vector3Int(obj.x + 8, obj.y -3));
                    if(obj.isXDirection)
                        instance = Instantiate(virus, basePos - new Vector3(0, 0.5f), Quaternion.identity, mapParent);
                    else
                        instance = Instantiate(virus, basePos - new Vector3(0.5f, 0), Quaternion.identity, mapParent);
                    instance.GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
           },
        };
    }
    void Start()
    {
        DrawMap();
        // TODO: ここでJson文字列をサーバーから受け取るイメージ
        var data = new List<MapObject>
        {
            new(0, 0, 1, Objects.Player, true),
            new(1, 1, 1, Objects.Resist, true),
            new(0, 0, 1, Objects.Lock, true),
            new(1, 0, 1, Objects.Ownership, true),
            new(0, 0, 1, Objects.Virus, false),
        };
        GameManager.Instance.mapObjectList = data;
        DrawMapObject();
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
    public void DrawMapObject()
    {
        var data = GameManager.Instance.mapObjectList;
        var xAdjustment = 8;
        var yAdjustment = -3;
        foreach (var item in data)
        {
            InstantiateMapObject(item, xAdjustment, yAdjustment);
        }
    }
    private void InstantiateMapObject(MapObject target, int xAdjustment, int yAdjustment)
    {

        if (mapObjectActionMap.TryGetValue(target.type, out var action))
        {
            action.Invoke(target);
        }
    }
}
