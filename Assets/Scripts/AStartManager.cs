using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStartManager
{
    public int mapW;
    public int mapH;
    
    private AStartNode[,] nodes;

    //开启列表
    private List<AStartNode> openList;
    //关闭列表
    private List<AStartNode> closeList;

    public void InitMapInfo(int w, int h)
    {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                AStartNode node = new AStartNode(i, j, Random.Range(0, 100) < 20 ? NodeType.Stop : NodeType.Walk);
            }
        }
    }

    public List<AStartNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        // 判断范围 省略
        // 判断阻挡 省略
        AStartNode start = nodes[(int) startPos.x, (int) startPos.y];
        AStartNode end = nodes[(int) endPos.x, (int) endPos.y];

        // 清空上次寻路数据
        closeList.Clear();
        openList.Clear();
        
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);

        while (true)
        {
            FindNearlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x, start.y - 1 , 1f, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
        
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y , 1.4f, start, end);
        
            FindNearlyNodeToOpenList(start.x - 1, start.y + 1 , 1.4f, start, end);
            FindNearlyNodeToOpenList(start.x, start.y +1, 1f, start, end);
            FindNearlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);

            if (openList.Count == 0)
            {
                return null;
            }
            
            openList.Sort(SortOpenList);
        
            closeList.Add(openList[0]);
            start = openList[0];
            openList.RemoveAt(0);

            if (start == end)
            {
                List<AStartNode> path = new List<AStartNode>();
                path.Add(end);
                while (end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                path.Reverse();
                return path;
            }
        }
    }

    private int SortOpenList(AStartNode a, AStartNode b)
    {
        if (a.f >= b.f)
            return 1;
        else
            return -1;
    }

    private void FindNearlyNodeToOpenList(int x, int y, float g, AStartNode father, AStartNode end)
    {
        // 边界判断
        if (x < 0 || x > mapW || y < 0 || y > mapH)
            return;
        
        AStartNode node = nodes[x, y];
        if (node == null || node.type == NodeType.Stop || closeList.Contains(node) || openList.Contains(node))
            return;
        
        // 计算f值 f = g + h
        node.father = father;
        // 计算g
        node.g = node.father.g + g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;
        
        
        openList.Add(node);
    }
}
