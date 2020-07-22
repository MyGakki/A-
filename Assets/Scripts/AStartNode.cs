using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AStartNode
{
    //坐标
    public int x;
    public int y;
    
    //寻路消耗
    public float f;
    //离起点的距离
    public float g;
    //离终点的距离
    public float h;
    //父对象
    public AStartNode father;
    //格子的类型
    public NodeType type;

    public AStartNode(int x, int y, NodeType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

}

public enum NodeType
{
    Walk,
    Stop
}
