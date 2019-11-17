/*
 * 이 스크립트가 부착된 게임 오브젝트의 position이 A* 알고리즘의 Target이 됩니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void Update()
    {
        GameObject.Find("Astar").GetComponent<PathFinding>().touchOrigin = (Vector2)transform.position;
    }
}
