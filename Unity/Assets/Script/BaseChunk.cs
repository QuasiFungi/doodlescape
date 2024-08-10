using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class BaseChunk : MonoBehaviour
{
    protected List<BaseChunk> _neighbours;
    protected Vector2Int _bounds;
    protected Vector3 _offset;
    // protected List<GameObject> _objects;    // mob prop item interact fluid sensor
    protected List<Entity> _entities;    // mob prop item interact fluid sensor
    protected int _state;   // 0 - player | 1 + neighbour
    protected Vector2Int offsetIndex;
    // * testing
    public static bool _showChunk = true;
    void Awake()
    {
        _neighbours = new List<BaseChunk>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        _bounds = new Vector2Int(Mathf.FloorToInt(collider.size.x), Mathf.FloorToInt(collider.size.y));
        _offset = collider.offset;
        // _objects = new List<GameObject>();
        _entities = new List<Entity>();
        // 
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
    void Start()
    {
        ManagerChunk.Instance.Register(this);
        // Load();
        // * testing
        // SetState(game_variables.Instance.Depth);
        SetState(1);
        // SetState(game_variables.Instance.Depth - 1);
        offsetIndex = GameGrid.Instance.WorldToIndex(transform.position + _offset - new Vector3(.5f, .5f));
    }
    // public void Initialize()
    // {
    //     // if (_state == game_variables.Instance.Depth)
    //     //     return;
    //     // if (_objects.Contains(controller_player.Instance.Data.gameObject))
    //     //     SetState(game_variables.Instance.Depth);
    //     // else
    //     // SetState(game_variables.Instance.Depth);
    //     if (_state > 0)
    //         SetState(1);
    //     // if (_state != 0)
    //     //     SetState(game_variables.Instance.Depth - 1);
    // }
    // initial scuffed coloration is just a visual bug, tested by changing to always gizmo
    void OnDrawGizmosSelected()
    // void OnDrawGizmos()
    {
        if (!_showChunk) return;
        // player
        if (_state == 0)
            Gizmos.color = new Color(0, 1, 0, .5f);
        // neighbour
        else if (_state > 0 && _state < GameVariables.Depth)
            Gizmos.color = new Color(0, 0, _state / (float)GameVariables.Depth, .5f);
        // off
        else if (_state == GameVariables.Depth)
            Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(transform.position + _offset, new Vector3(_bounds.x, _bounds.y, 1f));
    }
    void Update()
    {
        // // off
        // if (_state == game_variables.Instance.Depth)
        // {
        //     foreach (base_chunk chunk in _neighbours)
        //         // warmer
        //         if (chunk.State < _state - 1)
        //         {
        //             // warm
        //             SetState(_state - 1);
        //             break;
        //         }
        // }
        // // neighbour
        // else if (_state > 0 && _state < game_variables.Instance.Depth)
        // {
        //     int check = 0;
        //     foreach (base_chunk chunk in _neighbours)
        //         if (chunk.State < check)
        //             check = chunk.State;
        //     // if (check == 0)
        //     //     // demote
        //     //     SetState(_state - 1);
        //     // warmer
        //     if (check < _state)
        //         // cold
        //         SetState(check + 1);
        // }
        // ---
        // foreach (base_chunk chunk in _neighbours)
        //     // warmer
        //     if (chunk.State > 0 && chunk.State < _state)
        //     {
        //         // warm
        //         SetState(_state - 1);
        //         break;
        //     }
        //     // colder
        //     else if (chunk.State < game_variables.Instance.Depth && chunk.State > _state)
        //     {
        //         // cold
        //         SetState(_state + 1);
        //         break;
        //     }
        // ---
        // if (_state == 0)
        //     return;
        // int check = 0;
        // foreach (base_chunk chunk in _neighbours)
        //     if (_state == 0)
        //         chunk.SetState(_state + 1);
        //     else if (chunk.State < _state - 1)
        //         // promote
        //         check--;
        //     else if (chunk.State > _state + 1)
        //         // demote
        //         check++;
        // SetState(_state + check);
        // ---
        if (_state == 2)
        {
            foreach (BaseChunk chunk in _neighbours)
                if (chunk.State == 0)
                {
                    SetState(1);
                    break;
                }
        }
        else if (_state == 1)
        {
            bool check = true;
            foreach (BaseChunk chunk in _neighbours)
                if (chunk.State == 0)
                    check = false;
            if (check)
                SetState(2);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameVariables.IsPlayer(other.gameObject))
            // promote
            SetState(0);
        else if (other.gameObject.layer == GameVariables.LayerChunk)
        {
            BaseChunk temp = other.GetComponent<BaseChunk>();
            if (!_neighbours.Contains(temp))
                _neighbours.Add(temp);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (GameVariables.IsPlayer(other.gameObject))
            // demote
            SetState(1);
    }
    public void SetState(int value)
    {
        // ?
        if (_state == value)
            return;
        // (re)load objects
        // (re)load entities
        if (_state < GameVariables.Depth)
        {
            // restock
            // _objects.Clear();
            _entities.Clear();
            for (int x = -_bounds.x / 2; x < _bounds.x / 2; x++)
                for (int y = -_bounds.y / 2; y < _bounds.y / 2; y++)
                {
                    // * testing
                    Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + (Vector2)_offset + new Vector2(x, y) + Vector2.one * .5f, .45f, GameVariables.ScanLayerObject);
                    foreach (Collider2D collider in colliders)
                        if (collider.tag == "ignore")
                            continue;
                        else
                            // _objects.Add(collider.gameObject);
                            _entities.Add(collider.transform.GetComponent<Entity>());
                }
        }
        // apply
        // foreach (GameObject temp in _objects)
        //     temp?.SetActive(value < GameVariables.Depth);
        foreach (Entity entity in _entities)
            entity?.ToggleActive(value < GameVariables.Depth);
        // update
        _state = value;
    }
    public Vector2Int[] GetPositions()
    {
        Vector2Int[] positions = new Vector2Int[_bounds.x * _bounds.y];
        int index = 0;
        for (int x = 1 - _bounds.x / 2; x <= _bounds.x / 2; x++)
            for (int y = 1 -_bounds.y / 2; y <= _bounds.y / 2; y++)
            {
                positions[index] = offsetIndex + new Vector2Int(x, y);
                index++;
            }
        return positions;
    }
    public int State
    {
        get { return _state; }
    }
}