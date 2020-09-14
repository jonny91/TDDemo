//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-09-09 22:27:13
//Description: 
//=========================================

public class AStarNode
{
    #region 寻路属性

    public virtual float F { get; set; }
    public virtual float G { get; set; }
    public virtual float H { get; set; }
    public AStarNode Parent { get; set; }

    #endregion

    public int Row { protected set; get; }
    public int Column { protected set; get; }
    private AStarNodeType _nodeType;

    public AStarNode(int row, int column, AStarNodeType nodeType)
    {
        Row = row;
        Column = column;
        _nodeType = nodeType;
    }

    /// <summary>
    /// 非障碍点 可达
    /// </summary>
    /// <returns></returns>
    public bool Accessible()
    {
        return _nodeType != AStarNodeType.Disable;
    }

    public void Reset()
    {
        F = G = H = 0;
        Parent = null;
    }

    public override string ToString()
    {
        return $"{Row}-{Column}";
    }
}