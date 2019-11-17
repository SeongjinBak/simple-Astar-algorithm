/*
 * A* 알고리즘 커스텀 클래스 입니다.
 * 이 알고리즘은, 터치로 입력받은 장애물을 피해 target위치로 플레이어(seeker)를 가장 최적의 루트로 이동시키는 스크립트 입니다.
 * 
 * 이 클래스는 화면을 grids로 나누어야 합니다. 이를 위해 커스텀클래스 Grid와 Node를 사용합니다.
 * 
 * 사용시, Astar prefab을 Hierarchy view에 옮긴 후 position을 0,0,0으로 설정, 그리고
 * seeker에 Player를 등록하면 됩니다.
 */ 


using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathFinding : MonoBehaviour
{ 
    [Header("Path Finding")]
    public Transform seeker;
    // 맵을 격자로 분할한다.
    Grid grid;
    // 남은거리를 넣을 큐 생성.
    public Queue<Vector2> wayQueue = new Queue<Vector2>();

    [Header("Player Ctrl")]

    // 뭔가와 상호작용 하고 있을때는 walkable이 false 가 됨.
    public static bool walkable = true;
    // 터치 좌표를 저장할 변수
    public Vector2 touchOrigin;
    public Vector2 touchSave;
    private Quaternion playerRot;
    // 플레이어 이동/회전 속도 등 저장할 변수
    public float moveSpeed;
    public float rotSpeed;
    // 장애물/NPC 판단시 멈추게 할 범위
    public float range;

    public bool isWalk;
    public bool isWalking;

    private void Awake()
    {
        // 격자 생성
        grid = GetComponent<Grid>();
        walkable = true;
    }
    private void Start()
    {
        // 초깃값 초기화.
        isWalking = false;
        moveSpeed = 20f;
        rotSpeed = 5f;
        touchSave = seeker.position;
        touchOrigin = seeker.position;
        range = 4f;
        playerRot = transform.rotation;
        // story 0-1에는 캐릭터 자동이동이며, 이때 속도는 10으로 고정.
        if (SceneManager.GetActiveScene().name == "Story0-1")
            moveSpeed = 10f;
    }

    private void FixedUpdate()
    {
        isWalk = walkable;
        // walkable = true 인 경우에만 터치 받음 
        if (walkable == true)
        {
            CheckTouch();
        }
        // walkable이 false 면 제자리 멈춤
        else
        {
            touchSave = (Vector2)seeker.position;
        }
    }

    // 화면이 터치 되었는지 판단하는 함수.
    private void CheckTouch()
    {
        // 화면에 터치가 감지되었을 경우
        if (Input.touchCount > 0)
        {
            // 아래 코드는 화면 상에서 UI가 터치 되지 않았을 경우(즉, 버튼 등을 클릭하지 않았을 경우에만 움직이게 함.)
            // 무조건 해당 씬에 EventSystem이 있어야 함. 따라서, eventsystem이 없다면 추가해야 함.
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Touch myTouch = Input.touches[0];
                // 처음 터치 감지 | 마지막 터치 감지로 제한
                if ((myTouch.phase == TouchPhase.Began))
                {
                    touchOrigin = myTouch.position;
                }

                // 입력받은 터치를 화면좌표계 -> 월드좌표계로 변환
                touchOrigin = Camera.main.ScreenToWorldPoint(myTouch.position);
            }
        }

        // 이 객체를 터치된 지점까지 이동시킵니다.
        if(touchSave != touchOrigin)
        {
            touchSave = touchOrigin;
            //터치 초기 위치와 다른 좌표가 입력되었을 경우, 길찾기를 시작.
            StartFindPath(seeker.position, touchSave);
        }
    }

    // start to target 이동.
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(FindPath(startPos, targetPos));
    }

    // 길찾기 로직.
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {   
        // start, target의 좌표를 grid로 분할한 좌표로 지정.
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        
        // target에 도착했는지 확인하는 변수.
        bool pathSuccess = false;

        if (!startNode.walkable)
            Debug.LogError("Unwalkable StartNode 입니다.");

        // walkable한 targetNode인 경우 길찾기 시작.
        if(targetNode.walkable)
        {
            // openSet, closedSet 생성.
            // closedSet은 이미 계산 고려한 노드들.
            // openSet은 계산할 가치가 있는 노드들.
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            // closedSet에서 가장 최저의 F를 가지는 노드를 빼낸다. 
            while (openSet.Count > 0)
            {
                // currentNode를 계산 후 openSet에서 빼야 한다.
                Node currentNode = openSet[0];
                // 모든 openSet에 대해, current보다 f값이 작거나, h(휴리스틱)값이 작으면 그것을 current로 지정.
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                // openSet에서 current를 뺀 후, closedSet에 추가.
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // 방금 들어온 노드가 목적지 인 경우
                if (currentNode == targetNode)
                {
                    // seeker가 위치한 지점이 target이 아닌 경우
                    if(pathSuccess == false)
                    {
                       // wayQueue에 PATH를 넣어준다.
                       PushWay( RetracePath(startNode, targetNode) ) ;
                    }
                    pathSuccess = true;
                    break;
                }

                // current의 상하좌우 노드들에 대하여 g,h cost를 고려한다.
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;
                    // F cost 생성.
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // 이웃으로 가는 F cost가 이웃의 G보다 짧거나, 방문해볼 Openset에 그 값이 없다면,
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        // openSet에 추가.
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }
        else
        {
            // 잘 가다가 Unwalkable 오브젝트를 클릭할 경우 기존 PATH를 따라간다.
            // 그러나 way 최신화는 하지 않고 clear한다.
            Vector3 origin = seeker.position;
            while (true)
            {
                seeker.position = Vector2.MoveTowards(seeker.position, origin, moveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.03f);
                if ((int)seeker.position.x == (int)origin.x && (int)seeker.position.y == (int)origin.y) break;
            }

            wayQueue.Clear();
        }
        
        yield return null;

        // 길을 찾았을 경우(계산 다 끝난경우) 이동시킴.
        if(pathSuccess == true)
        {
            // 이동중이라는 변수 ON
            isWalking = true;
            // wayQueue를 따라 이동시킨다.
            while (wayQueue.Count > 0)
            {
                seeker.position = Vector2.MoveTowards(seeker.position, wayQueue.First(), moveSpeed * Time.deltaTime);
                if ((Vector2)seeker.position == wayQueue.First())
                {
                    wayQueue.Dequeue();
                }
                yield return new WaitForSeconds(0.02f);
            }
            // 이동중이라는 변수 OFF
            isWalking = false;
        }
    }

    // WayQueue에 새로운 PATH를 넣어준다.
    void PushWay(Vector2[] array)
    {
        wayQueue.Clear();
        foreach (Vector2 item in array)
        {
            wayQueue.Enqueue(item);
        }
    }

    // 현재 큐에 거꾸로 저장되어있으므로, 역순으로 wayQueue를 뒤집어준다. 
    Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode  = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        // Grid의 path에 찾은 길을 등록한다.
        grid.path = path;
        Vector2[] wayPoints = SimplifyPath(path);
        return wayPoints;
    }

    // Node에서 Vector 정보만 빼낸다.
    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> wayPoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 0; i < path.Count; i++)
        {
            wayPoints.Add(path[i].worldPosition);
        }
        return wayPoints.ToArray();
    }

    // custom g cost 또는 휴리스틱 추정치를 계산하는 함수.
    // 매개변수로 들어오는 값에 따라 기능이 바뀝니다.
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // 대각선 - 14, 상하좌우 - 10.
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
