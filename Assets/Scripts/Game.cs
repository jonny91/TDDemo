//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020/09/14 23:42:40
//Description: 
//=========================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private AStarMap _map;

    [SerializeField]
    private GameObject StartPrefab;

    [SerializeField]
    private GameObject EndPrefab;

    [SerializeField]
    private Vector2Int StartPos;

    [SerializeField]
    private Vector2Int EndPos;

    private float _gridCell = 0.1f;
    private int _rowCount = 100;
    private int _columnCount = 100;

    private void Awake()
    {
        _map = new AStarMap();
    }

    private void Start()
    {
        _map.InitMap(_columnCount, _rowCount);

        DrawStartEndPos(StartPos, StartPrefab);
        DrawStartEndPos(EndPos, EndPrefab);
    }

    private void DrawStartEndPos(Vector2Int pos, GameObject prefab)
    {
        var p = Parse2Point(pos.x, pos.y);
        Instantiate(prefab, p, Quaternion.identity);
    }

    private Vector3 Parse2Point(int row, int column)
    {
        return new Vector3(
            (column - _columnCount / 2) * _gridCell + _gridCell / 2,
            0,
            (row - _rowCount / 2) * _gridCell + +_gridCell / 2);
    }

//    private Vector2 ParseFromPoint(Vector3 p)
//    {
//        var x = p.x;
//        var y = p.y;
//        var columnIndex = (int) ((x - 0.5 * _gridCell) / _gridCell) + (x > 0 ? 1 : -1);
//        var rowIndex = (int) ((y - 0.5 * _gridCell) / _gridCell) + (y > 0 ? 1 : -1);
//        return new Vector2(rowIndex, columnIndex);
//    }
}