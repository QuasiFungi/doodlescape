using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class BaseChunk : MonoBehaviour
{
    private List<BaseChunk> _neighbours;
    private Vector2Int _bounds;
    private Vector3 _offset;
    // protected List<GameObject> _objects;    // mob prop item interact fluid sensor
    private List<Entity> _entities;    // mob prop item interact fluid sensor
    public enum Type
    {
        PLAYER,
        NEIGHBOUR,
        OFF
    }
    private Type _state = Type.PLAYER;   // 0 - player | 1 + neighbour
    private Vector2Int offsetIndex;
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
        _room = new Vector2Int(Mathf.FloorToInt(transform.position.x / 9f), Mathf.FloorToInt(transform.position.y / 9f));
        // 
        // LoadEntities();
        // StateUpdate(Vector2Int.zero);
    }
    void Start()
    {
        ManagerChunk.Instance.Register(this);
        // ? in start vs awake, here so not interfere with entity loot system
        LoadEntities();
        StateUpdate(Vector2Int.zero);
        // Load();
        // * testing
        // SetState(game_variables.Instance.Depth);
        // SetState(1);
        // SetState(2);
        // print(Time.time + "start");
        // SetState(Type.NEIGHBOUR);
        // ? use player position from save/load data
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
    // void OnDrawGizmosSelected()
    void OnDrawGizmos()
    {
        if (!_showChunk) return;
        // // player
        // if (_state == 0)
        //     Gizmos.color = new Color(0, 1, 0, .5f);
        // // neighbour
        // else if (_state > 0 && _state < GameVariables.Depth)
        //     Gizmos.color = new Color(0, 0, _state / (float)GameVariables.Depth, .5f);
        // // off
        // else if (_state == GameVariables.Depth)
        //     Gizmos.color = new Color(1, 0, 0, .5f);
        switch (_state)
        {
            case Type.PLAYER:
                Gizmos.color = new Color(0, 1, 0, .5f);
                break;
            case Type.NEIGHBOUR:
                Gizmos.color = new Color(0, 0, 1, .5f);
                break;
            case Type.OFF:
                Gizmos.color = new Color(1, 0, 0, .5f);
                break;
        }
        Gizmos.DrawCube(transform.position + _offset, new Vector3(_bounds.x, _bounds.y, 1f));
    }
    // void Update()
    // {
    //     // // off
    //     // if (_state == game_variables.Instance.Depth)
    //     // {
    //     //     foreach (base_chunk chunk in _neighbours)
    //     //         // warmer
    //     //         if (chunk.State < _state - 1)
    //     //         {
    //     //             // warm
    //     //             SetState(_state - 1);
    //     //             break;
    //     //         }
    //     // }
    //     // // neighbour
    //     // else if (_state > 0 && _state < game_variables.Instance.Depth)
    //     // {
    //     //     int check = 0;
    //     //     foreach (base_chunk chunk in _neighbours)
    //     //         if (chunk.State < check)
    //     //             check = chunk.State;
    //     //     // if (check == 0)
    //     //     //     // demote
    //     //     //     SetState(_state - 1);
    //     //     // warmer
    //     //     if (check < _state)
    //     //         // cold
    //     //         SetState(check + 1);
    //     // }
    //     // ---
    //     // foreach (base_chunk chunk in _neighbours)
    //     //     // warmer
    //     //     if (chunk.State > 0 && chunk.State < _state)
    //     //     {
    //     //         // warm
    //     //         SetState(_state - 1);
    //     //         break;
    //     //     }
    //     //     // colder
    //     //     else if (chunk.State < game_variables.Instance.Depth && chunk.State > _state)
    //     //     {
    //     //         // cold
    //     //         SetState(_state + 1);
    //     //         break;
    //     //     }
    //     // ---
    //     // if (_state == 0)
    //     //     return;
    //     // int check = 0;
    //     // foreach (base_chunk chunk in _neighbours)
    //     //     if (_state == 0)
    //     //         chunk.SetState(_state + 1);
    //     //     else if (chunk.State < _state - 1)
    //     //         // promote
    //     //         check--;
    //     //     else if (chunk.State > _state + 1)
    //     //         // demote
    //     //         check++;
    //     // SetState(_state + check);
    //     // ---
    //     if (_state == 2)
    //     {
    //         foreach (BaseChunk chunk in _neighbours)
    //             if (chunk.State == 0)
    //             {
    //                 SetState(1);
    //                 break;
    //             }
    //     }
    //     else if (_state == 1)
    //     {
    //         bool check = true;
    //         foreach (BaseChunk chunk in _neighbours)
    //             if (chunk.State == 0)
    //                 check = false;
    //         if (check)
    //             SetState(2);
    //     }
    // }
    // // proactive over reactive, toggle between neighbour <> off states
    // private void UpdateNeighbours(bool promote)
    // {
    //     switch (_state)
    //     {
    //         case Type.OFF:
    //             if (!promote) return;
    //             // player in vicinity
    //             foreach (BaseChunk chunk in _neighbours)
    //                 // promote
    //                 chunk.SetState(Type.NEIGHBOUR);
    //             break;
    //         case Type.NEIGHBOUR:
    //             if (promote) return;
    //             // player not in vicinity
    //             foreach (BaseChunk chunk in _neighbours)
    //                 // demote
    //                 SetState(Type.OFF);
    //             break;
    //     }
    // }
    void OnEnable()
    {
        // GameClock.onTickChunk += LocalState;
        GameMaster.onTransitionBegin += StateUpdate;
    }
    void OnDisable()
    {
        // GameClock.onTickChunk -= LocalState;
        GameMaster.onTransitionBegin += StateUpdate;
    }
    private Vector2Int _room = Vector2Int.zero;
    // private void LocalState(Vector2Int room)
    private void StateUpdate(Vector2Int room)
    {
        // print(room + " " + _room);
        float distance = Vector2Int.Distance(room, _room);
        // if (distance == 0f) SetState(Type.PLAYER);
        // else if (distance < 2f) SetState(Type.NEIGHBOUR);
        // else SetState(Type.OFF);
        SetState(distance == 0f ? Type.PLAYER : (distance < 2f ? Type.NEIGHBOUR : Type.OFF));
        // print(_entities.Count);
        // print(Time.time + "local");
        // switch (_state)
        // {
        //     case Type.OFF:
        //         // player in vicinity
        //         foreach (BaseChunk chunk in _neighbours)
        //             if (chunk.IsState(Type.PLAYER))
        //             {
        //                 // promote
        //                 SetState(Type.NEIGHBOUR);
        //                 break;
        //             }
        //         break;
        //     case Type.NEIGHBOUR:
        //         // player not in vicinity
        //         bool check = true;
        //         foreach (BaseChunk chunk in _neighbours)
        //             if (chunk.IsState(Type.PLAYER)) check = false;
        //         // demote
        //         if (check) SetState(Type.OFF);
        //         break;
        // }
    }
    // public delegate void OnPlayerDetect();
    // public static event OnPlayerDetect onPlayerDetect;
    void OnTriggerEnter2D(Collider2D other)
    {
        // // player entered
        // if (GameVariables.IsPlayer(other.gameObject))
        // {
        //     // promote
        //     // SetState(0);
        //     SetState(Type.PLAYER);
        //     // print(Time.time + "trigger");
        //     // UpdateNeighbours(true);
        //     // // update neighbours
        //     // foreach (BaseChunk chunk in _neighbours) SetState(Type.NEIGHBOUR);
        //     // // notify manager of player movement ? all chunks/entities are active on first call lol
        //     // onPlayerDetect?.Invoke();
        // }
        // // ? called during first frame only
        // else if (other.gameObject.layer == GameVariables.LayerChunk)
        // {
        //     BaseChunk temp = other.GetComponent<BaseChunk>();
        //     if (!_neighbours.Contains(temp))
        //         _neighbours.Add(temp);
        // }
        // ? called during first frame only
        if (other.gameObject.layer == GameVariables.LayerChunk)
        {
            BaseChunk temp = other.GetComponent<BaseChunk>();
            if (!_neighbours.Contains(temp))
                _neighbours.Add(temp);
        }
    }
    // void OnTriggerExit2D(Collider2D other)
    // {
    //     // player left
    //     if (GameVariables.IsPlayer(other.gameObject))
    //     {
    //         // demote
    //         // SetState(1);
    //         // SetState(Type.NEIGHBOUR);
    //         // UpdateNeighbours();
    //         // // update neighbours
    //         // foreach (BaseChunk chunk in _neighbours) SetState(Type.NEIGHBOUR);
    //         // // notify manager of player movement
    //         // onPlayerDetect?.Invoke();
    //     }
    // }
    private void LoadEntities()
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
                    // exempt walls and player from tracked entites
                    if (collider.tag == "ignore" || collider.tag == "player")
                        continue;
                    else
                        // _objects.Add(collider.gameObject);
                        _entities.Add(collider.transform.GetComponent<Entity>());
            }
    }
    // public void SetState(int value)
    private void SetState(Type state)
    {
        // ? not sure if even called ? when remain neighbour etc
        // if (_state == value) return;
        if (_state == state) return;
        // update
        // _state = value;
        else _state = state;
        // // do state update even if unchanged
        // _state = state;
        // apply
        // foreach (GameObject temp in _objects)
        //     temp?.SetActive(value < GameVariables.Depth);
        foreach (Entity entity in _entities)
            // entity?.ToggleActive(value < GameVariables.Depth);
            // ? use separate function for chunk loading
            // entity?.ToggleLoad(IsActive);
            entity?.ToggleActive(IsActive, true);
        // (re)load objects
        // (re)load entities only when chunk active ? possible some not covered by capture
        // if (_state < GameVariables.Depth)
        if (IsActive) LoadEntities();
    }
    // tiles contained within this chunk
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
    public List<Entity> GetEntitesActive()
    {
        List<Entity> entities = new List<Entity>();
        foreach (Entity entity in _entities)
            if (entity && entity.IsActive) entities.Add(entity);
        return entities;
    }
    // public bool IsState(Type state)
    // {
    //     return _state == state;
    // }
    // public int State
    // {
    //     get { return _state; }
    // }
    public bool IsActive
    {
        // get { return _state < GameVariables.Depth; }
        get { return _state != Type.OFF; }
    }
}