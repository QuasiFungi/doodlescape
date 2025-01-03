using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using System;
using System.Collections.Generic;
public class GameNavigation : MonoBehaviour
{
    #region Action
    public static int GetTileAtPosition(Vector2 position, out GameObject tile)
    {
        // 
        Collider2D hit = Physics2D.OverlapCircle(position, .1f, GameVariables.ScanLayerAction);
        // allow actions if valid tile detected that isn't trigger ? can pathfind over hidden creature
        if (hit != null && !hit.isTrigger)
        {
            tile = hit.gameObject;
            return tile.gameObject.layer - 1;
        }
        tile = null;
        return 0;
    }
    public static bool IsTileAtPosition(Vector2 position)
    {
        // 
        Collider2D hit = Physics2D.OverlapCircle(position, .1f, GameVariables.ScanLayerAction);
        // detected entity other than sensor that isn't also trigger
        return hit != null && !hit.isTrigger && hit.gameObject.layer != GameVariables.LayerSensor;
    }
    #endregion
    Queue<PathResult> results = new Queue<PathResult>();
    // job calculate path
    // grid > native
    // native > path
    public static GameNavigation Instance;
    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }
    void Update()
    {
        if (results.Count > 0)
        {
            for (int i = 0; i < results.Count; i++)
            {
                PathResult result = results.Dequeue();
                result.callback(result.path, result.success);
            }
        }
    }
    // #region ActionFilter
    // // entites to ignore actions from
    // private List<FilterAction> _filter = new List<FilterAction>();
    // private struct FilterAction
    // {
    //     public string id;
    //     public GameAction.ActionType type;
    //     public FilterAction(string id, GameAction.ActionType type)
    //     {
    //         this.id = id;
    //         this.type = type;
    //     }
    // }
    // private void FilterModify(string id, GameAction.ActionType type, bool state)
    // {
    //     if (state)
    //     {
    //         if (FilterContains(id, type)) return;
    //         // 
    //         _filter.Add(new FilterAction(id, type));
    //     }
    //     else FilterRemove(id, type);
    // }
    // private bool FilterContains(string id, GameAction.ActionType type)
    // {
    //     // match on entity ID and action type pair ? multiple action block possible
    //     foreach (FilterAction filter in _filter) if (filter.id == id && filter.type == type) return true;
    //     return false;
    // }
    // private void FilterRemove(string id, GameAction.ActionType type)
    // {
    //     // FilterAction temp;
    //     foreach (FilterAction filter in _filter)
    //     {
    //         // temp = filter;
    //         // break;
    //         if (filter.id == id && filter.type == type)
    //         {
    //             _filter.Remove(filter);
    //             break;
    //         }
    //     }
    // }
    // void OnEnable()
    // {
    //     HitboxIntercept.onIntercept += FilterModify;
    // }
    // void OnDisable()
    // {
    //     HitboxIntercept.onIntercept -= FilterModify;
    // }
    // #endregion
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public void PathCalculate(PathData request)
    // public void PathCalculate(PathData request, string id)
    {
        // // verify new action should be ignored
        // if (FilterContains(id, GameAction.ActionType.WALK))
        // {
        //     // dummmy data
        //     results.Enqueue(new PathResult(new Vector3[0], false, request.callback));
        //     // abort
        //     return;
        // }
        // 
        NativeList<int2> path = new NativeList<int2>(1, Allocator.TempJob);
        NativeArray<PathNode> pathNodeArray = GridToNative(new int2((int)request.target.x, (int)request.target.y));
        PathCalculateJob pathCalculateJob = new PathCalculateJob
        {
            startPosition = new int2((int)request.source.x, (int)request.source.y),
            endPosition = new int2((int)request.target.x, (int)request.target.y),
            path = path,
            pathNodeArray = pathNodeArray,
            size = new int2(GameGrid.Instance.SizeX, GameGrid.Instance.SizeY),
            isDiagonal = request.isDiagonal,
        };
        JobHandle jobHandle = pathCalculateJob.Schedule();
        jobHandle.Complete();
        // Vector3[] waypoints = NativeToVector3(pathCalculateJob.path);
        Vector3[] waypoints = NativeToVector3(pathCalculateJob.path.AsArray());
        // // * testing
        // print(waypoints.Length);
        path.Dispose();
        pathNodeArray.Dispose();
        results.Enqueue(new PathResult(waypoints, waypoints.Length > 0, request.callback));
    }
    private NativeArray<PathNode> GridToNative(int2 endPosition)
    {
        GridData[,] grid = GameGrid.Instance.Grid;
        NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(grid.GetLength(0) * grid.GetLength(1), Allocator.TempJob);
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                PathNode pathNode = new PathNode();
                pathNode.x = x;
                pathNode.y = y;
                pathNode.index = CalculateIndex(x, y, grid.GetLength(0));
                pathNode.costG = int.MaxValue;
                pathNode.costH = CalculateDistanceCost(new int2(x, y), endPosition);
                pathNode.CalculateCostF();
                pathNode.isWalkable = grid[x, y].IsWalkable;
                pathNode.weight = grid[x, y].Weight;
                pathNode.indexPrev = -1;
                pathNodeArray[pathNode.index] = pathNode;
            }
        }
        return pathNodeArray;
    }
    private Vector3[] NativeToVector3(NativeArray<int2> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        foreach (int2 waypoint in path)
        {
            waypoints.Add(GameGrid.Instance.IndexToWorld(waypoint.x, waypoint.y));
        }
        return PathSimplify(waypoints);
    }
    private Vector3[] PathSimplify(List<Vector3> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            // Vector2 directionNew = new Vector2(path[i - 1].x - path[i].x, path[i - 1].y - path[i].y);
            // if (directionNew != directionOld)
            // {
                waypoints.Add(path[i - 1]);
            // }
            // directionOld = directionNew;
        }
        Vector3[] temp = waypoints.ToArray();
        Array.Reverse(temp);
        return temp;
    }
    private int CalculateIndex(int x, int y, int gridWidth)
    {
        return x + y * gridWidth;
    }
    private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
    {
        int xDistance = math.abs(aPosition.x - bPosition.x);
        int yDistance = math.abs(aPosition.y - bPosition.y);
        int remaining = math.abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    [BurstCompile]
    private struct PathCalculateJob : IJob
    {
        public int2 startPosition;
        public int2 endPosition;
        public NativeList<int2> path;
        public NativeArray<PathNode> pathNodeArray;
        public int2 size;
        public bool isDiagonal;
        public void Execute()
        {
            // burst compatible
            NativeArray<int2> neighbourOffsetArray;
            if (isDiagonal)
            {
                neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
                neighbourOffsetArray[0] = new int2(-1, 0);  // L
                neighbourOffsetArray[1] = new int2(+1, 0);  // R
                neighbourOffsetArray[2] = new int2(0, -1);  // D
                neighbourOffsetArray[3] = new int2(0, +1);  // U
                neighbourOffsetArray[4] = new int2(-1, -1); // LD
                neighbourOffsetArray[5] = new int2(-1, +1); // LU
                neighbourOffsetArray[6] = new int2(+1, -1); // RD
                neighbourOffsetArray[7] = new int2(+1, +1); // DU
            }
            else
            {
                neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp);
                neighbourOffsetArray[0] = new int2(-1, 0);  // L
                neighbourOffsetArray[1] = new int2(+1, 0);  // R
                neighbourOffsetArray[2] = new int2(0, -1);  // D
                neighbourOffsetArray[3] = new int2(0, +1);  // U
            }
            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, size.x);
            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, size.x)];
            startNode.costG = 0;
            startNode.CalculateCostF();
            pathNodeArray[startNode.index] = startNode;
            NativeList<int> listOpen = new NativeList<int>(Allocator.Temp);
            NativeList<int> listClosed = new NativeList<int>(Allocator.Temp);
            listOpen.Add(startNode.index);
            while (listOpen.Length > 0)
            {
                int currentNodeIndex = GetLowestCostFNodeIndex(listOpen, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];
                if (currentNodeIndex == endNodeIndex)
                {
                    // reached end
                    break;
                }
                // remove current node from open list
                for (int i = 0; i < listOpen.Length; i++)
                {
                    if (listOpen[i] == currentNodeIndex)
                    {
                        listOpen.RemoveAtSwapBack(i);
                        break;
                    }
                }
                listClosed.Add(currentNodeIndex);
                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);
                    if (!IsPositionInsideGrid(neighbourPosition, size))
                    {
                        // neighbour not valid position
                        continue;
                    }
                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, size.x);
                    if (listClosed.Contains(neighbourNodeIndex))
                    {
                        // already searched this node
                        continue;
                    }
                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if (!neighbourNode.isWalkable)
                    {
                        // not walkable
                        continue;
                    }
                    // diagonal neighbour
                    if (Mathf.Abs(neighbourOffset[0]) + Mathf.Abs(neighbourOffset[1]) > 1)
                    {
                        // corners blocked
                        if (!(pathNodeArray[CalculateIndex(neighbourPosition.x, currentNode.y, size.x)].isWalkable || pathNodeArray[CalculateIndex(currentNode.x, neighbourPosition.y, size.x)].isWalkable))
                            continue;
                    }
                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);
                    int tentativeCostG = currentNode.costG + CalculateDistanceCost(currentNodePosition, neighbourPosition) + neighbourNode.weight;
                    // if (tentativeCostG < neighbourNode.costG)
                    if (tentativeCostG < neighbourNode.costG || !listOpen.Contains(neighbourNodeIndex))
                    {
                        neighbourNode.indexPrev = currentNodeIndex;
                        neighbourNode.costG = tentativeCostG;
                        neighbourNode.CalculateCostF();
                        pathNodeArray[neighbourNodeIndex] = neighbourNode;
                        if (!listOpen.Contains(neighbourNode.index))
                        {
                            listOpen.Add(neighbourNode.index);
                        }
                    }
                }
            }
            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.indexPrev == -1)
            {
                // path NOT found
                // print("het");
            }
            else PathCalculate(pathNodeArray, endNode);
            neighbourOffsetArray.Dispose();
            listOpen.Dispose();
            listClosed.Dispose();
        }
        private int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }
        private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
        {
            int xDistance = math.abs(aPosition.x - bPosition.x);
            int yDistance = math.abs(aPosition.y - bPosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }
        private void PathCalculate(NativeArray<PathNode> pathNodeArray, PathNode endNode)
        {
            if (endNode.indexPrev == -1)
            {
                // path NOT found
                return;
            }
            else
            {
                // path found
                path.Add(new int2(endNode.x, endNode.y));
                PathNode currentNode = endNode;
                while (currentNode.indexPrev != -1)
                {
                    PathNode nodePrev = pathNodeArray[currentNode.indexPrev];
                    path.Add(new int2(nodePrev.x, nodePrev.y));
                    currentNode = nodePrev;
                }
            }
        }
        private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }
        private int GetLowestCostFNodeIndex(NativeList<int> listOpen, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestCostPathNode = pathNodeArray[listOpen[0]];
            for (int i = 1; i < listOpen.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[listOpen[i]];
                if (testPathNode.costF < lowestCostPathNode.costF)
                {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }
    }
    // value type
    public struct PathNode
    {
        public int x;
        public int y;
        public int index;
        public int costG;
        public int costH;
        public int costF;
        public bool isWalkable;
        public int weight;
        public int indexPrev;
        public void CalculateCostF()
        {
            costF = costG + costH;
        }
    }
    // * testing null error on reload
    void OnDestroy()
    {
        Instance = null;
    }
}
public struct PathData
{
    public Vector2Int source;
    public Vector2Int target;
    public bool isDiagonal;
    public Action<Vector3[], bool> callback;
    public PathData(Vector3 source, Vector3 target, bool isDiagonal, Action<Vector3[], bool> callback)
    {
        this.source = GameGrid.Instance.WorldToIndex(source);
        this.target = GameGrid.Instance.WorldToIndex(target);
        this.isDiagonal = isDiagonal;
        this.callback = callback;
    }
}
public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;
    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}