//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-09-09 22:28:29
//Description: 
//=========================================

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AStarMap
{
    private int _width;
    private int _height;

    /// <summary>
    /// 所有节点
    /// </summary>
    private AStarNode[,] _nodes;

    private List<AStarNode> _openList = new List<AStarNode>();
    private List<AStarNode> _closeList = new List<AStarNode>();
    private List<AStarNode> _path = new List<AStarNode>();

    private AStarNode _endNode;

    public void InitMap(int width, int height)
    {
        _nodes = new AStarNode[width, height];
        _width = width;
        _height = height;
        //创建地图节点
        CreateNode();
    }

    protected virtual void CreateNode()
    {
        for (int column = 0; column < _width; column++)
        {
            for (int row = 0; row < _height; row++)
            {
                var node = new AStarNode(row, column, AStarNodeType.Enable);
                _nodes[column, row] = node;
            }
        }
    }

    /// <summary>
    /// 寻找路径
    /// </summary>
    /// <param name="startIndexPos"></param>
    /// <param name="endIndexPos"></param>
    /// <returns></returns>
    public virtual List<AStarNode> FindPath(Vector2Int startIndexPos, Vector2Int endIndexPos)
    {
        if (!PosAvaliable(startIndexPos) || !PosAvaliable(endIndexPos))
        {
            return null;
        }

        //起点
        var startNode = GetNode(startIndexPos.x, startIndexPos.y);
        //终点
        _endNode = GetNode(endIndexPos.x, endIndexPos.y);

        if (!startNode.Accessible() || !_endNode.Accessible())
        {
            return null;
        }

        startNode.Parent = null;
        _closeList.Add(startNode);

        while (startNode != _endNode)
        {
            //left
            FindNearlyNodeToOpenList(startNode.Row - 1, startNode.Column, 1, startNode, _endNode);
            //right
            FindNearlyNodeToOpenList(startNode.Row + 1, startNode.Column, 1, startNode, _endNode);
            //up
            FindNearlyNodeToOpenList(startNode.Row, startNode.Column - 1, 1, startNode, _endNode);
            //down
            FindNearlyNodeToOpenList(startNode.Row, startNode.Column + 1, 1, startNode, _endNode);

            if (_openList.Count == 0)
            {
                return null;
            }

            //升序排列
            _openList.Sort(Sort);

            var pathNode = _openList[0];
            //从OpenList中移到CloseList
            _closeList.Add(pathNode);
            _openList.RemoveAt(0);

            startNode = pathNode;
        }

        //最终路径
        while (true)
        {
            _path.Add(startNode);
            if (startNode.Parent == null)
            {
                break;
            }

            startNode = startNode.Parent;
        }

        _path.Reverse();
        return _path;
    }

    private int Sort(AStarNode a, AStarNode b)
    {
        if (a.F > b.F)
        {
            return 1;
        }
        else if (a.F < b.F)
        {
            return -1;
        }
        else
        {
            var blockCountA = GetBlockCountInRow(a.Row, a.Column);
            var blockCountB = GetBlockCountInRow(b.Row, b.Column);
            if (blockCountA > blockCountB)
            {
                return 1;
            }
            else if (blockCountA < blockCountB)
            {
                return -1;
            }
            else
            {
                if (a.Parent.Row == a.Row && b.Parent.Row != b.Row)
                {
                    return -1;
                }
                else if (a.Parent.Row != a.Row && b.Parent.Row == b.Row)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    private int GetBlockCountInRow(int x, int y)
    {
        int count = 0;
        var minY = Math.Min(y, _endNode.Column);
        var maxY = Math.Max(y, _endNode.Column);
        for (int i = minY; i < maxY; i++)
        {
            var node = GetNode(x, i);
            if (!node.Accessible())
            {
                count++;
            }
        }

        return count;
    }

    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode parentNode, AStarNode endNode)
    {
        if (!PosAvaliable(x, y))
        {
            return;
        }

        var node = GetNode(x, y);
        if (_closeList.Contains(node) || _openList.Contains(node))
        {
            return;
        }

        if (!node.Accessible())
        {
            //障碍
            return;
        }

        node.Parent = parentNode;
        node.G = parentNode.G + g;
        //曼哈顿街区算法
        node.H = Math.Abs(x - endNode.Row) + Math.Abs(y - endNode.Column);
        node.F = node.G + node.H;

        //加入Open列表
        _openList.Add(node);
    }

    /// <summary>
    /// 目标点合法性判断
    /// </summary>
    /// <param name="indexPos"></param>
    /// <returns></returns>
    private bool PosAvaliable(Vector2Int indexPos)
    {
        return PosAvaliable(indexPos.x, indexPos.y);
    }

    private bool PosAvaliable(int x, int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }

    public AStarNode GetNode(int x, int y)
    {
        return _nodes[x, y];
    }

    /// <summary>
    /// 重置所有数据
    /// </summary>
    public void Reset()
    {
        _width = _height = 0;
        _nodes = null;
        ResetFindPath();
    }

    public void ResetFindPath()
    {
        _path.Clear();
        _openList.Clear();
        _closeList.Clear();

        foreach (var aStarNode in _nodes)
        {
            aStarNode.Reset();
        }
    }
}