using UnityEngine;
using System.Collections.Generic;
public class GameGrid : MonoBehaviour
{
    // * testing
    public bool showGrid;
    protected Vector2Int _weightBounds = new Vector2Int(int.MaxValue, int.MinValue);
    void OnDrawGizmos()
    {
        if (!showGrid)
            return;
        // Gizmos.DrawWireCube(transform.position + _offset, new Vector3(_sizeGrid.x, _sizeGrid.y));
        for (int x = 0; x < _sizeGrid_Int.x; x++)
        {
           for (int y = 0; y < _sizeGrid_Int.y; y++)
           {
                // Gizmos.color = _grid[x, y].IsWalkable ? Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(_weightBounds.x, _weightBounds.y, _grid[x, y].Weight)) : Color.red;
                Gizmos.color = _grid[x, y].IsWalkable ? Color.black : Color.red;
                Gizmos.DrawWireCube(IndexToWorld(x, y), Vector3.one * .45f);
           }
        }
    }
    // static layer
    // - solid
    // - fluid
    // dynamic layer
    // - item
    // - interact
    // - mob
    public static GameGrid Instance;
    // [static]
    public struct GridTile
    {
        // position [world]
        public Vector3 Position;
        // walkable
        public bool IsWalkable;
        // weight
        public int Weight;
    }
    [SerializeField] protected Vector2 _sizeGrid;
    protected Vector2Int _sizeGrid_Int;
    // [SerializeField] protected float _sizeTile = 1f;
    [SerializeField] protected Vector3 _offset;
    // [SerializeField] protected float _correction = 0.1f;
    protected bool[,] _gridStatic;
    protected int[,] _gridDynamic;
    protected GridData[,] _grid;
    // weight
    [System.Serializable]
    public class SurfaceType
    {
        public LayerMask Mask;
        public int Weight;
    }
    [SerializeField] protected SurfaceType[] _surfacesDynamic;
    protected LayerMask _maskDynamic;
    protected Dictionary<int, int> _surfacesDictionary = new Dictionary<int, int>();
    void Awake()
    {
        // singleton
        if (Instance) Destroy(gameObject);
        else Instance = this;
        // weight [dynamic]
        foreach (SurfaceType surface in _surfacesDynamic)
        {
            _maskDynamic.value |= surface.Mask.value;
            _surfacesDictionary.Add((int)Mathf.Log(surface.Mask.value, 2), surface.Weight);
        }
    }
    void Start()
    {
        // grid [static]
        // _sizeGrid_Int = new Vector2Int(Mathf.RoundToInt(_sizeGrid.x / _sizeTile), Mathf.RoundToInt(_sizeGrid.y / _sizeTile));
        _sizeGrid_Int = new Vector2Int(Mathf.RoundToInt(_sizeGrid.x), Mathf.RoundToInt(_sizeGrid.y));
        _gridStatic = new bool[_sizeGrid_Int.x, _sizeGrid_Int.y];
        for (int x = 0; x < _sizeGrid_Int.x; x++)
            for (int y = 0; y < _sizeGrid_Int.y; y++)
                _gridStatic[x, y] = Physics2D.OverlapCircle(IndexToWorld(x, y), .1f, GameVariables.ScanLayerSolid) == null;
        // grid [dynamic]
        _gridDynamic = new int[_sizeGrid_Int.x, _sizeGrid_Int.y];
        // UpdateGridDynamic(null);
        // grid [data]
        _grid = new GridData[_sizeGrid_Int.x, _sizeGrid_Int.y];
        // UpdateGrid(null);
    }
    // * testing [? dev control variable]
    [SerializeField] private float _time = 1f;
    private float _timer = .5f;
    void Update()
    {
        if (_timer > 0)
            _timer -= Time.deltaTime;
        else
        {
            _timer = _time;
            List<Vector2Int[]> chunks = ManagerChunk.Instance.GetActive();
            if (chunks.Count == 0) return;
            UpdateGridDynamic(chunks);
            UpdateGrid(chunks);
        }
    }
    private void UpdateGridDynamic(List<Vector2Int[]> chunks)
    {
        foreach (Vector2Int[] chunk in chunks)
            foreach (Vector2Int position in chunk)
            {
                // print(position);
                int weight = 0;
                Collider2D collider = Physics2D.OverlapCircle(IndexToWorld(position.x, position.y), .1f, _maskDynamic);
                if (collider != null)
                {
                    // // item equipped ?
                    // if (collider.gameObject.layer == GameVariables.LayerItem && collider.transform.parent)
                    //     weight = 0;
                    // fly/ground type ?
                    // else if (collider.gameObject.layer == GameVariables.LayerCreature && collider.isTrigger)
                    if (collider.gameObject.layer == GameVariables.LayerCreature && collider.isTrigger)
                        weight = 0;
                    else
                        _surfacesDictionary.TryGetValue(collider.gameObject.layer, out weight);
                }
                _gridDynamic[position.x, position.y] = weight;
            }
        // for (int x = 0; x < _sizeGrid_Int.x; x++)
        // {
        //     for (int y = 0; y < _sizeGrid_Int.y; y++)
        //     {
        //         int weight = 0;
        //         Collider2D collider = Physics2D.OverlapCircle(IndexToWorld(x, y), .1f, _maskDynamic);
        //         if (collider != null)
        //         {
        //             // item equipped ?
        //             if (collider.gameObject.layer == game_variables.Instance.LayerItem && collider.transform.parent)
        //                 weight = 0;
        //             // fly/ground type ?
        //             else if (collider.gameObject.layer == game_variables.Instance.LayerMob && collider.isTrigger)
        //                 weight = 0;
        //             else
        //                 _surfacesDictionary.TryGetValue(collider.gameObject.layer, out weight);
        //         }
        //         // GridTile tile = new GridTile();
        //         // tile.Position = initialize ? IndexToWorld(x, y) : tile.Position;
        //         // tile.IsWalkable = Physics2D.OverlapCircle(tile.Position, .1f, game_variables.Instance.ScanLayerNotWalkable) == null;
        //         // tile.Weight = weight;
        //         // _gridDynamic[x, y] = tile;
        //         _gridDynamic[x, y] = weight;
        //     }
        // }
    }
    private void UpdateGrid(List<Vector2Int[]> chunks)
    {
        foreach (Vector2Int[] chunk in chunks)
            foreach (Vector2Int position in chunk)
           {
                GridData data = new GridData();
                data.IsWalkable = _gridStatic[position.x, position.y] && (_gridDynamic[position.x, position.y] != -1);
                // print(data.IsWalkable);
                data.Weight = _gridDynamic[position.x, position.y];
                _grid[position.x, position.y] = data;
                // * testing
                if (data.Weight < _weightBounds.x)
                _weightBounds.x = data.Weight;
                if (data.Weight > _weightBounds.y)
                _weightBounds.y = data.Weight;
           }
        // for (int x = 0; x < _sizeGrid_Int.x; x++)
        // {
        //    for (int y = 0; y < _sizeGrid_Int.y; y++)
        //    {
        //         GridData data = new GridData();
        //         data.IsWalkable = _gridStatic[x, y] && (_gridDynamic[x, y] != -1);
        //         // print(data.IsWalkable);
        //         data.Weight = _gridDynamic[x, y];
        //         _grid[x, y] = data;
        //         // // * testing
        //         // if (data.Weight < _weightBounds.x)
        //         // _weightBounds.x = data.Weight;
        //         // if (data.Weight > _weightBounds.y)
        //         // _weightBounds.y = data.Weight;
        //    }
        // }
    }
    public Vector3 IndexToWorld(int x, int y)
    {
        return Vector3.right * (x + 0.5f) + Vector3.up * (y + 0.5f) + _offset - Vector3.right * _sizeGrid.x / 2f - Vector3.up * _sizeGrid.y / 2f;
    }
    public Vector2Int WorldToIndex(Vector3 position)
    {
        // axis
        position -= Vector3.right * 0.5f + Vector3.up * 0.5f + _offset - Vector3.right * _sizeGrid.x / 2f - Vector3.up * _sizeGrid.y / 2f;
        // // scale
        // position /= _sizeTile;
        // bounds
        position.x = Mathf.Clamp(position.x, 0, _sizeGrid.x - 1);
        position.y = Mathf.Clamp(position.y, 0, _sizeGrid.y - 1);
        // 
        return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }
    public Vector3 WorldToGrid(Vector3 position)
    {
        Vector2Int temp = WorldToIndex(position);
        return IndexToWorld(temp.x, temp.y);
    }
    // public bool IsWalkable(Vector3 position)
    // {
    //     Vector2Int index = WorldToIndex(position);
    //     return _grid[index.x, index.y].IsWalkable;
    // }
    public GridData[,] Grid
    {
        get { return _grid; }
    }
    public int SizeX
    {
        get { return _sizeGrid_Int.x; }
    }
    public int SizeY
    {
        get { return _sizeGrid_Int.y; }
    }
}
// 
public struct GridData
{
    public bool IsWalkable;
    public int Weight;
}