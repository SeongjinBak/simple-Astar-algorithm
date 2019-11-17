/*
 * 커스텀 Node 클래스 입니다.
 * 
 * Grid 안에 들어갈 Node입니다.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    // 길 되추적을 위한 parent변수.
    public Node parent;

    // Node 생성자.
    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    // F cost 계산 속성.
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
