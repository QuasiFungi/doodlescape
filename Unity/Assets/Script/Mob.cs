using UnityEngine;
using System.Collections.Generic;
using Panda;
public class Mob : Creature
{
    void OnEnable()
    {
        // ? after player has moved
        GameClock.onTick += BehaviourTick;
    }
    void OnDisable()
    {
        GameClock.onTick -= BehaviourTick;
    }
    private PandaBehaviour _ai;
    [Tooltip("ai tick tied to clock (vs frame)")] [SerializeField] private bool _isAI = true;
    private void BehaviourTick()
    {
        // get latest info
        UpdateState();
        // act on new info ? creatures that tick every frame are taxing
        if (_isAI) _ai.Tick();
    }
    protected bool _isAlert;
    protected bool _isAware;
    protected float _awareTimer;
    // time since sensor event
    protected float _sensorTimer;
    // ignore/forget older sounds ? events
    [Tooltip("ignore/forget older sensor events")] [SerializeField] protected float _attentionTime;
    // ? feels ugly to use, enums maybe
    [Tooltip("enable/disable all sensors")] [SerializeField] protected bool _isSensors;
    [Tooltip("enable/disable trigger sensors only")] [SerializeField] protected bool _isTrigger;
    [Tooltip("enable/disable vision sensors only")] [SerializeField] protected bool _isVision;
    // * testing ? get automatically from children
    [SerializeField] protected TestVision[] _sensorsVision;
    [SerializeField] protected List<TestTrigger> _sensorsTrigger;
    protected class EventPoint
    {
        public Vector3 Position;
        public LayerMask Layer;
        public float Time;
        // public EventPoint()
        // {
        //     // * testing
        // }
        public EventPoint(Vector3 position, LayerMask layer, float time)
        {
            Position = position;
            Time = time;
        }
    }
    protected EventPoint _anchor;
    protected EventPoint _positionVision;
    protected EventPoint _positionTrigger;
    // [SerializeField] protected List<Waypoint> _waypoints;
    // // * testing
    // protected List<Transform> _hostiles;
    // [Tooltip("colors this entity can take")] [SerializeField] protected Color[] _colors;
    [Tooltip("Number of internal timers")] [SerializeField] protected int _countTimers;
    protected float[] _timers;
    [Tooltip("Number of internal flags")] [SerializeField] protected int _countFlags;
    protected bool[] _flags;
    // * testing [attack ? effect]
    [Tooltip("Attacks, effects, etc")] [SerializeField] protected GameObject[] _prefabs;
    [Tooltip("Prefab spawn points")] [SerializeField] protected Transform[] _spawns;
    // * testing load sensors trigger
    [Tooltip("id of external sensor trigger")] [SerializeField] protected string _trigger = "";
    private enum TriggerSearchType
    {
        NULL,
        BOUNDS,
        TREE
    }
    [Tooltip("method of searching for trigger sensors")] [SerializeField] private TriggerSearchType _searchType = TriggerSearchType.NULL;
    [Tooltip("Sensors check area")] [SerializeField] protected Vector2Int _bounds = Vector2Int.one;
    [Tooltip("Check area offset")] [SerializeField] protected Vector2 _offset = Vector2.zero;
    // * testing
    protected List<Vector2> _test = new List<Vector2>();
    [Tooltip("highlight tiles being searched for sensor triggers")] [SerializeField] private bool _showSearch = false;
    // for asyncronous movement
    protected float _timerPath;
    // ? use string ids
    [Tooltip("ids of entities that will be attacked")] [SerializeField] protected List<string> _hostiles;
    // ? taken from anim or entity
    protected SpriteRenderer _sprite;
    [Tooltip("Mob sprite variants (0 is default)")] [SerializeField] protected Sprite[] _sprites;
    protected List<Collider2D> _colliders;
    // 
    protected Color[] _colors;
    protected float _rotation;
    protected float _speed = 30f;
    void Awake()
    {
        _ai = GetComponent<PandaBehaviour>();
        // 
        _anchor = new EventPoint(transform.position, gameObject.layer, 0f);
        // _BT = GetComponent<PandaBehaviour>();
        // _isBT = false;
        _isAlert = false;
        _isAware = false;
        _awareTimer = 0;
        _sensorTimer = _attentionTime;
        _isVision = true;
        _isTrigger = true;
        _isSensors = true;
        _positionVision = null;
        _positionTrigger = null;
        _timers = new float[_countTimers];
        _flags = new bool[_countFlags];
        // _sleep = true;
        _timerPath = 0f;
        // _flagStatus = -1;
        // _hostiles = new List<Transform>();
        // 
        switch (_searchType)
        {
            case TriggerSearchType.BOUNDS:
                for (float x = .5f-_bounds.x / 2f; x <= -.5f+_bounds.x / 2f; x++)
                    for (float y = .5f-_bounds.y / 2f; y <= -.5f+_bounds.y / 2f; y++)
                    {
                        // * testing detection align with grid
                        if (_showSearch) _test.Add((Vector2)transform.position + new Vector2(x, y) + _offset);
                        TestTrigger temp = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(x, y) + _offset, .1f, GameVariables.ScanLayerSensor)?.transform.GetComponent<TestTrigger>();
                        if (temp?.transform.GetComponent<Entity>().ID == _trigger && !_sensorsTrigger.Contains(temp)) _sensorsTrigger.Add(temp);
                    }
                break;
            case TriggerSearchType.TREE:
                // - list of positions to search listSearch
                // - listSearch default to 8 adjacent positions to mob
                // - take nth position from listSearch, default 0
                // - check if contains sensorTrigger
                // - if does
                // -    add to listSensor
                // -    iterate 8 adjacent positions to this position
                // -    if not already in listSearch add to listSearch
                // - iterate n
                // - if n less than listSearch size, repeat from step 3
                List<Vector2> listSearch = new List<Vector2>();
                Vector2 position = (Vector2)transform.position;
                LoadAdjacentTiles(position, ref listSearch);
                int count = 0;
                while (count < listSearch.Count)
                {
                    position = listSearch[count];
                    // * testing positions being searched
                    if (_showSearch) _test.Add(position);
                    TestTrigger temp = Physics2D.OverlapCircle(position, .1f, GameVariables.ScanLayerSensor)?.transform.GetComponent<TestTrigger>();
                    if ((temp?.transform.GetComponent<Entity>().ID == _trigger && !_sensorsTrigger.Contains(temp)))
                    {
                        _sensorsTrigger.Add(temp);
                        LoadAdjacentTiles(position, ref listSearch);
                    }
                    count++;
                }
                break;
        }
        foreach (TestTrigger sensor in _sensorsTrigger)
            sensor.SetActive(true);
        // ? move to anim
        _sprite = GetComponent<SpriteRenderer>();
        // * testing ? move to motor
        _colliders = new List<Collider2D>();
        _colliders.Add(GetComponent<Collider2D>());
        // 
        // preset
        _colors = new Color[2];
        _colors[0] = new Color(0f, 1f, 0f, 1f);
        _colors[1] = new Color(.25f, .25f, .25f, 1f);
        _rotation = transform.eulerAngles.z;
    }
    private void LoadAdjacentTiles(Vector2 position, ref List<Vector2> list)
    {
        // up
        position += Vector2.up;
        if (!list.Contains(position)) list.Add(position);
        // up right
        position += Vector2.right;
        if (!list.Contains(position)) list.Add(position);
        // right
        position += Vector2.down;
        if (!list.Contains(position)) list.Add(position);
        // down right
        position += Vector2.down;
        if (!list.Contains(position)) list.Add(position);
        // down
        position += Vector2.left;
        if (!list.Contains(position)) list.Add(position);
        // down left
        position += Vector2.left;
        if (!list.Contains(position)) list.Add(position);
        // left
        position += Vector2.up;
        if (!list.Contains(position)) list.Add(position);
        // up left
        position += Vector2.up;
        if (!list.Contains(position)) list.Add(position);
    }
    void Update()
    {
        // ? memory waste, assume time passed is 1 second since last updatestate, use int timer for ticks
        if (ProcessVision() || ProcessTrigger()) _awareTimer += _attentionTime * Time.deltaTime;
        // 
        else if (_awareTimer > 0) _awareTimer -= Time.deltaTime;
        // ? check if not already max
        _sensorTimer = Mathf.Clamp(_sensorTimer + Time.deltaTime, 0, _attentionTime);
        // 
        for (int i = 0; i < _countTimers; i++) if (_timers[i] > 0f) _timers[i] -= Time.deltaTime;
        // 
        if (_timerPath > 0) _timerPath -= Time.deltaTime;
        // 
        // * testing ? high speed jitter ? handle overshoot
        if (Mathf.Abs(_rotation) > _speed * Time.deltaTime)
            transform.eulerAngles += Vector3.forward * Mathf.Sign(_rotation) * _speed * Time.deltaTime;
        
    }
    private void UpdateState()
    {
        // 
        if (_awareTimer > _attentionTime)
        {
            _awareTimer = _attentionTime;
            _isAware = true;
        }
        // 
        else if (_awareTimer < 0)
        {
            _awareTimer = 0;
            _isAware = false;
        }
        // 
        if (_positionTrigger != null && Time.time - _positionTrigger.Time > _attentionTime) _positionTrigger = null;
        if (_positionVision != null && Time.time - _positionVision.Time > _attentionTime) _positionVision = null;
    }
    // ? executed twice first in behaviour tree then update tick
    protected bool ProcessVision()
    {
        if (!_isVision || !_isSensors)
            return false;
        float distance = float.MaxValue;
        foreach (TestVision sensor in _sensorsVision)
        {
            foreach (Transform target in sensor.Targets)
            {
                if (!_hostiles.Contains(target.GetComponent<Entity>().ID)) continue;
                // ? closest [memory waste ?]
                if (Vector3.Distance(transform.position, target.position) < distance)
                {
                    distance = Vector3.Distance(transform.position, target.position);
                    _positionVision = new EventPoint(target.position, target.gameObject.layer, Time.time);
                }
            }
            // print(gameObject.name + "\t" + sensor.Targets.Count);
        }
        if (_positionVision != null)
        {
            // // enable waypoints ? borked ?
            // EnableWaypoints();
            _isAlert = true;
            _sensorTimer = 0;
            // *testing instant aware
            if (_isAware)
                _awareTimer = _attentionTime;
            return true;
        }
        return false;
    }
    // ? executed twice first in behaviour tree then update tick
    protected bool ProcessTrigger()
    {
        if (!_isTrigger || !_isSensors)
            return false;
        float distance = float.MaxValue;
        foreach (TestTrigger sensor in _sensorsTrigger)
        {
            foreach (Transform target in sensor.Targets)
            {
                // if (!_hostiles.Contains(target))
                //     continue;
                if (!_hostiles.Contains(target.GetComponent<Entity>().ID)) continue;
                // * testing exclude dead existing
                if (target.gameObject.layer == GameVariables.LayerCreature)
                    if (target.GetComponent<Creature>().HealthInst <= 0)
                        continue;
                // ? closest [memory waste ?]
                if (Vector3.Distance(transform.position, target.position) < distance)
                {
                    distance = Vector3.Distance(transform.position, target.position);
                    _positionTrigger = new EventPoint(target.position, target.gameObject.layer, Time.time);
                }
            }
            // print(gameObject.name + "\t" + sensor.Targets.Count);
        }
        if (_positionTrigger != null)
        {
            // // enable waypoints ? borked ?
            // EnableWaypoints();
            _isAlert = true;
            _sensorTimer = 0;
            // *testing instant aware
            _awareTimer = _attentionTime;
            return true;
        }
        return false;
    }
    // * testing snap
    private void SetTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float from = transform.eulerAngles.z % 360f;
        from = from < 0 ? from + 360f : from;
        float to = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f) % 360f;
        to = to < 0 ? to + 360f : to;
        _rotation = to - from;
        _rotation = Mathf.Abs(_rotation) > 180 ? -_rotation % 180 : _rotation;
    }
    private bool CheckDirection(Vector3 target, float value)
    {
        Vector3 direction = (target - transform.position).normalized;
        float angle = (270f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360f;
        if (angle > transform.eulerAngles.z)
            return angle - transform.eulerAngles.z <= value;
        return transform.eulerAngles.z - angle <= value;
    }
    // * testing move
    void OnDrawGizmos()
    {
        if (_searchType == TriggerSearchType.BOUNDS)
        {
            Gizmos.color = Color.red;
            // Gizmos.DrawWireCube(_motor.Position + (Vector3)_offset, new Vector3(_bounds.x, _bounds.y));
            Gizmos.DrawWireCube(transform.position + (Vector3)_offset, new Vector3(_bounds.x - .1f, _bounds.y - .1f));
        }
        if (_searchType != TriggerSearchType.NULL)
        {
            // detected trigger sensors
            foreach (TestTrigger sensor in _sensorsTrigger)
            {
                Gizmos.color = new Color(1f, 0f, 0f, .5f);
                Gizmos.DrawCube(sensor.transform.position, Vector3.one * .9f);
            }
        }
        // ensure trigger detection align to grid
        foreach (Vector2 test in _test)
        {
            Gizmos.color = new Color(1f, 0f, 0f, .5f);
            Gizmos.DrawCube(test, Vector3.one * .9f);
        }
        // anchor
        if (_anchor != null)
        {
            Gizmos.color = new Color(1f, 1f, 1f, .5f);
            Gizmos.DrawSphere(_anchor.Position, .5f);
            Gizmos.DrawLine(transform.position, _anchor.Position);
        }
    }
    #region Behaviour Tree
    // [Task]
    // void Testing(string value)
    // {
    //     print(value);
    //     ThisTask.Succeed();
    // }
    [Task]
    void DoAttack(int indexPrefab, int indexSpawn, int parent)
    {
        // // * testing [? safety]
        // _motor.NavigateCancel();
        // _anim.SetTarget(_anchor.Position);
        // 
        GameObject temp = Instantiate(_prefabs[indexPrefab], _spawns[indexSpawn].position, _spawns[indexSpawn].rotation);
        // 
        temp.GetComponent<BaseHitbox>().Initialize(this as Breakable);
        // 
        if (parent == 1) temp.transform.SetParent(_spawns[indexSpawn]);
        // 
        temp.SetActive(true);
        // 
        ThisTask.Succeed();
    }
    [Task]
    void EntitySpeed(float value)
    {
        // _motor.Speed = value;
        // if (value == 0 && _motor.Speed != 0)
        //     _motor.NavigateCancel();
        ThisTask.Succeed();
    }
    [Task]
    bool IsAlert
    {
        get { return _isAlert; }
    }
    [Task]
    void IsAlive()
    {
        ThisTask.debugInfo = HealthInst.ToString();
        ThisTask.Complete(HealthInst > 0);
    }
    [Task]
    void IsBored()
    {
        ThisTask.debugInfo = _sensorTimer + " : " + _attentionTime;
        ThisTask.Complete(_sensorTimer == _attentionTime);
    }
    [Task]
    void IsTimer(int index)
    {
        if (index > -1 && index < _countTimers)
        {
            ThisTask.debugInfo = _timers[index].ToString();
            ThisTask.Complete(_timers[index] > 0f);
        }
        else
            ThisTask.Succeed();
    }
    [Task]
    void IsDirection(float value)
    {
        // * testing [? safety]
        // _motor.NavigateCancel();
        SetTarget(_anchor.Position);
        ThisTask.Complete(CheckDirection(_anchor.Position, value));
    }
    [Task]
    void MoveToTrigger()
    {
        if (_positionTrigger == null) ThisTask.Fail();
        else
        {
            ThisTask.debugInfo = _positionTrigger.Position.ToString();
            // * testing [? chase bounds/limit]
            // _anchor.position = _positionTrigger.Position;
            _anchor = _positionTrigger;
            _positionTrigger = null;
            ThisTask.Succeed();
        }
    }
    [Task]
    void SensorAny()
    {
        ThisTask.debugInfo = "S:" + _isSensors + " V:" + _isVision + " T:" + _isTrigger;
        // check sensors [priority]
        ThisTask.Complete(ProcessTrigger() || ProcessVision());
    }
    [Task]
    void SetColor(int index)
    {
        if (index > -1 && index < _colors.Length)
        {
            _sprite.color = _colors[index];
            ThisTask.Succeed();
        }
        else ThisTask.Fail();
    }
    [Task]
    void SetColliders(int value)
    {
        // * testing
        foreach (Collider2D collider in _colliders) collider.enabled = value == 1;
        // _motor.CollidersToggle(value == 1);
        ThisTask.Succeed();
    }
    // * testing
    [Task]
    void SetSpeedRotation(float value)
    {
        _speed = value;
        ThisTask.Succeed();
    }
    [Task]
    void SetSprite(int index)
    {
        if (index > -1 && index < _sprites.Length)
        {
            // ? rename as renderer
            _sprite.sprite = _sprites[index];
            // _anim.SetSprite(value);
            ThisTask.Succeed();
        }
        else ThisTask.Fail();
    }
    [Task]
    void SetTimer(int index, float value)
    {
        if (index > -1 && index < _countTimers)
        {
            _timers[index] = value;
            ThisTask.Succeed();
        }
        else ThisTask.Fail();
    }
    [Task]
    void UnsetAlert()
    {
        _isAlert = false;
        ThisTask.Succeed();
    }
    #endregion
}