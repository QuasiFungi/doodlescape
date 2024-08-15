using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Panda;
public class Mob : Creature
{
    void OnEnable()
    {
        // ? after player has moved
        // GameClock.onTick += BehaviourTick;
        GameClock.onTickLate += BehaviourTick;
        // GameAction.onTransition += ToggleAI;
        GameMaster.onTransitionComplete += ToggleMob;
    }
    void OnDisable()
    {
        // GameClock.onTick -= BehaviourTick;
        GameClock.onTickLate -= BehaviourTick;
        // GameAction.onTransition -= ToggleAI;
        GameMaster.onTransitionComplete += ToggleMob;
    }
    private PandaBehaviour _ai;
    [Tooltip("ai tick tied to clock (vs frame)")] [SerializeField] private bool _isAI = true;
    private void BehaviourTick()
    {
        // * testing room loading
        if (_disableMob) return;
        // get latest info
        UpdateState();
        // act on new info ? creatures that tick every frame are taxing
        if (_isAI) _ai.Tick();
    }
    private bool _disableMob = true;
    // all mobs belong to maximum two domains ? bosses exception
    private Vector2Int[] _domain = new Vector2Int[2];
    protected float _spawnRotation;
    private void ToggleMob(Vector2Int room)
    {
        // // * testing ? unbounded, glitch possible
        // _enableAI = !_enableAI;
        // * testing ? case for mobs on border ? multi room domain, all domains mismatched
        // _disableMob = room != _domain[0];
        _disableMob = room != _domain[0] && room != _domain[1];
        // reset mob state
        if (_disableMob)
        {
            // move to spawn position
            SetPosition(_spawn);
            // set rotation to spawn default
            transform.eulerAngles = Vector3.forward * _spawnRotation;
            _body.eulerAngles = transform.eulerAngles;
            // // cancel navigation
            // // SetSpeedMove(0f);
            // NavigateCancel();
            // ? reset bt execution
            // _ai.Reset();
            // reset dynamic state parameters
            InitializeState();
            // ? waypoint info
            if (_isNavigate)
                // ? position change can result in following wrong waypoint group, inspector assigned waypoint group/filter
                _waypoint = ManagerWaypoint.Instance.GetWaypointNearest(Position);
            // bt specific parameters like collider sprite color vision trigger, offload logic to bt? redundant block?
            // ?
            SetColliders(true);
            _sprite.sprite = _sprites[0];
            _sprite.color = _colors[0];
            // // apply default state to sensors, use other explicit method for sensor debug?
            // foreach(SensorTrigger sensor in _sensorsTrigger) sensor.gameObject.SetActive(_isTrigger);
            // foreach(SensorVision sensor in _sensorsVision) sensor.gameObject.SetActive(_isVision);
            // ? spawned effects like attack particle, delete/pool
        }
        // * testing
        // foreach (SensorTrigger sensor in _sensorsTrigger) sensor.ToggleActive(!_disableMob);
        // foreach (SensorVision sensor in _sensorsVision) sensor.ToggleFOV(!_disableMob);
        foreach (SensorVision sensor in _sensorsVision) sensor.ToggleActive(!_disableMob);
    }
    protected bool _isAlert;
    protected bool _isAware;
    protected int _awareTimer;
    // time since sensor event
    protected int _sensorTimer;
    // ignore/forget older sounds ? events
    [Tooltip("ignore/forget older sensor events")] [SerializeField] protected int _attentionTime;
    // ? feels ugly to use, enums maybe
    [Tooltip("enable/disable all sensors")] [SerializeField] protected bool _isSensors;
    // protected bool _useSensors;
    [Tooltip("enable/disable trigger sensors only")] [SerializeField] protected bool _isTrigger;
    protected bool _useTrigger;
    [Tooltip("enable/disable vision sensors only")] [SerializeField] protected bool _isVision;
    protected bool _useVision;
    // * testing ? get automatically from children
    [SerializeField] protected SensorVision[] _sensorsVision;
    [SerializeField] protected List<SensorTrigger> _sensorsTrigger;
    protected class EventPoint
    {
        public Vector3 Position;
        public LayerMask Layer;
        public float Time;
        public EventPoint()
        {
            // * testing
        }
        public EventPoint(Vector3 position, LayerMask layer, float time)
        {
            Position = position;
            Time = time;
        }
    }
    protected EventPoint _anchor;
    protected EventPoint _positionVision;
    protected EventPoint _positionTrigger;
    // // * testing
    // protected List<Transform> _hostiles;
    // [Tooltip("colors this entity can take")] [SerializeField] protected Color[] _colors;
    [Tooltip("Number of internal timers")] [SerializeField] protected int _countTimers;
    protected int[] _timers;
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
    // ? use string ids
    [Tooltip("ids of entities that will be attacked")] [SerializeField] protected List<string> _hostiles;
    [Tooltip("Mob sprite variants (0 is default)")] [SerializeField] protected Sprite[] _sprites;
    // 
    protected Color[] _colors;
    protected float _rotation;
    protected float _speedTurn = 0f;
    // initialize static state parameters
    protected override void Awake()
    {
        base.Awake();
        // 
        _ai = GetComponent<PandaBehaviour>();
        // 
        // _anchor = new EventPoint(Position, gameObject.layer, 0f);
        // // _BT = GetComponent<PandaBehaviour>();
        // // _isBT = false;
        // _isAlert = false;
        // _isAware = false;
        // _awareTimer = 0;
        // _sensorTimer = _attentionTime;
        // 
        // _isSensors = true;
        // _isTrigger = true;
        _useTrigger = _isTrigger;
        // _isVision = true;
        _useVision = _isVision;
        if ((!_useTrigger && !_useVision) || !_isSensors) Debug.LogWarning(gameObject.name + ":\tMob has no active sensors");
        // 
        // _positionVision = null;
        // _positionTrigger = null;
        // _timers = new int[_countTimers];
        // _flags = new bool[_countFlags];
        InitializeState();
        // 
        // _sleep = true;
        // _timerPath = 0f;
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
                        if (_showSearch) _test.Add((Vector2)Position + new Vector2(x, y) + _offset);
                        SensorTrigger temp = Physics2D.OverlapCircle((Vector2)Position + new Vector2(x, y) + _offset, .1f, GameVariables.ScanLayerSensor)?.transform.GetComponent<SensorTrigger>();
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
                Vector2 position = (Vector2)Position;
                LoadAdjacentTiles(position, ref listSearch);
                int count = 0;
                while (count < listSearch.Count)
                {
                    position = listSearch[count];
                    // * testing positions being searched
                    if (_showSearch) _test.Add(position);
                    SensorTrigger temp = Physics2D.OverlapCircle(position, .1f, GameVariables.ScanLayerSensor)?.transform.GetComponent<SensorTrigger>();
                    if ((temp?.transform.GetComponent<Entity>().ID == _trigger && !_sensorsTrigger.Contains(temp)))
                    {
                        _sensorsTrigger.Add(temp);
                        LoadAdjacentTiles(position, ref listSearch);
                    }
                    count++;
                }
                break;
        }
        // // ? why do this
        // foreach (SensorTrigger sensor in _sensorsTrigger)
        //     sensor.SetActive(true);
        // 
        // 
        // preset
        _colors = new Color[2];
        _colors[0] = new Color(0f, 1f, 0f, 1f);
        _colors[1] = new Color(.25f, .25f, .25f, 1f);
        // _rotation = transform.eulerAngles.z;
        // _rotation = 0f;
        // 
        // - MOTOR
        // 
        // _path = Position;
        // _move = Position;
        _spawn = Position;
        _spawnRotation = transform.eulerAngles.z;
        // _directionCheck = 0f;
        // _directionTarget = Position;
        // 
        // - ANIM
        // 
        _lerp = new AnimationCurve();
        _lerp.AddKey(0f, 0f);
        _lerp.AddKey(.25f, 1f);
        _lerp.AddKey(.75f, -1f);
        _lerp.AddKey(1f, 0f);
        // 
        // _lerp.SmoothTangents(0, -1f);
        // _lerp.SmoothTangents(1, -1f);
        // _lerp.SmoothTangents(2, -1f);
        // _lerp.SmoothTangents(3, -1f);
        // 
        // _lerp = AnimationCurve.Linear(0f, 0f, .25f, 1f);
        // _lerp += AnimationCurve.Linear(.25f, 1f, .75f, -1f);
        // _lerp.Linear(.75f, -1f, 1f, 0f);
        // 
        List<Keyframe> keyframes = new List<Keyframe>();
        for (int i = 0; i < _lerp.length; i++)
        {
            Keyframe frame = _lerp[i];
            if (i > 0&& i != _lerp.length - 1)
            {
                var nextFrame=_lerp[i+1];
                var prefFrame=_lerp[i-1];
                float inTangent = (float) (((double) frame.value - (double) prefFrame.value) / ((double) frame.time - (double)  prefFrame.time ));
                float outTangent = (float) (((double) nextFrame.value - (double) frame.value) / ((double) nextFrame.time - (double)  frame.time ));
                frame.inTangent = inTangent;
                frame.outTangent = outTangent;
            }
            else
            {
                if (i == 0)
                {
                    var nextFrame=_lerp[i+1];
                    float outTangent = (float) (((double) nextFrame.value - (double) frame.value) / ((double) nextFrame.time - (double)  frame.time ));
                    frame.outTangent = outTangent;
                }
                else if (i == _lerp.length - 1)
                {
                    var prefFrame=_lerp[i-1];
                    float inTangent = (float) (((double) frame.value - (double) prefFrame.value) / ((double) frame.time - (double)  prefFrame.time ));
                    frame.inTangent = inTangent;
                }
            }
            keyframes.Add(frame);
        }
        _lerp.keys = keyframes.ToArray();
    }
    // dynamic state parameters ? pass in state data
    private void InitializeState()
    {
        _anchor = new EventPoint(Position, gameObject.layer, 0f);
        _isAlert = false;
        _isAware = false;
        _awareTimer = 0;
        _sensorTimer = _attentionTime;
        // ? sensors to default
        _positionVision = null;
        _positionTrigger = null;
        // ? new vs set value, garbage collector load
        _timers = new int[_countTimers];
        _flags = new bool[_countFlags];
        // 
        _rotation = 0f;
        // 
        _path = Position;
        _directionCheck = 0f;
        _directionTarget = Position;
    }
    protected override void Start()
    {
        base.Start();
        // 
        if (_isNavigate)
        {
            // ? position change can result in following wrong waypoint group
            _waypoint = ManagerWaypoint.Instance.GetWaypointNearest(Position);
            // just in case
            if (_waypoint == null) Debug.LogWarning(gameObject.name + ":\tRequires waypoint but failed to find one");
        }
        // 
        // convert spawn position to room position
        // - offset by (5, 5)
        // - round off by 10
        // _domain = new Vector2Int((_spawn.x + 5f) / 10f, (_spawn.y + 5f) / 10f);
        // if (gameObject.name == "mob_fortress_spikeSnail")
        //     print(gameObject.name + ": " + Mathf.FloorToInt((_spawn.x + 4.5f) / 9f) + ", " + Mathf.FloorToInt((_spawn.y + 4.5f) / 9f));
        // get primary domain
        _domain[0] = new Vector2Int(Mathf.FloorToInt((_spawn.x + 4.5f) / 9f), Mathf.FloorToInt((_spawn.y + 4.5f) / 9f));
        // get secondary domain
        // - initialize to default
        _domain[1] = _domain[0];
        // - get adjusted spawn
        Vector2 offset = new Vector2(_spawn.x + 4.5f, _spawn.y + 4.5f);
        // - check if on border
        if (offset.x % 9f == 0f) _domain[1] += _domain[0].x * 9f > _spawn.x ? Vector2Int.left : Vector2Int.right;
        else if (offset.y % 9f == 0f) _domain[1] += _domain[0].y * 9f > _spawn.y ? Vector2Int.down : Vector2Int.up;
        // * testing ? use player position/room from save data
        ToggleMob(Vector2Int.zero);
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
        // print(CheckDirection());
        // 
        if (_testRotate && !CheckDirection())
        // if (_testRotate && !_directionCheck)
        {
            // 
            GetRotation();
            // print(_rotation);
            // print("rotate?");
            // * testing ? high speed jitter ? handle overshoot
            if (Mathf.Abs(_rotation) > _speedTurn * Time.deltaTime)
                transform.eulerAngles += Vector3.forward * Mathf.Sign(_rotation) * _speedTurn * Time.deltaTime;
            else
                transform.eulerAngles += Vector3.forward * _rotation * Time.deltaTime;
            _body.eulerAngles = transform.eulerAngles;
        }
        // 
        // * testing scan vision
        // if (_scanTimer > 0f) _scanTimer -= Time.deltaTime;
        // if (_scanTimer > 0) _scanTimer--;
    }
    private void UpdateState()
    {
        // get latest sensor info
        ProcessTrigger();
        ProcessVision();
        // recall sensor information [priority]
        if (CheckTrigger() || CheckVision())
        {
            // prevent overshoot since trigger can set aware
            if (_awareTimer < _attentionTime) _awareTimer++;
        }
        else if (_awareTimer > 0) _awareTimer--;
        // 
        if (_awareTimer == _attentionTime) _isAware = true;
        else if (_awareTimer == 0) _isAware = false;
        // ? check if not already max
        _sensorTimer = Mathf.Clamp(_sensorTimer + 1, 0, _attentionTime);
        // 
        if (_positionTrigger != null && Time.time - _positionTrigger.Time > (float)_attentionTime) _positionTrigger = null;
        if (_positionVision != null && Time.time - _positionVision.Time > (float)_attentionTime) _positionVision = null;
        // 
        for (int i = 0; i < _countTimers; i++) if (_timers[i] > 0f) _timers[i]--;
        // 
        if (_timerPath > 0) _timerPath--;
    }
    // ? executed twice first in behaviour tree then update tick
    protected void ProcessVision()
    {
        // is vision allowed
        if (!_useVision || !_isSensors) return;
        float distance = float.MaxValue;
        foreach (SensorVision sensor in _sensorsVision)
        {
            foreach (Transform target in sensor.Targets)
            {
                if (!_hostiles.Contains(target.GetComponent<Entity>().ID)) continue;
                // ? closest [memory waste ?]
                if (Vector3.Distance(Position, target.position) < distance)
                {
                    distance = Vector3.Distance(Position, target.position);
                    _positionVision = new EventPoint(target.position, target.gameObject.layer, Time.time);
                }
            }
            // print(gameObject.name + "\t" + sensor.Targets.Count);
        }
    }
    private bool CheckVision()
    {
        // is vision allowed
        if (!_useVision || !_isSensors) return false;
        if (_positionVision != null)
        {
            // // enable waypoints ? borked ?
            // EnableWaypoints();
            _isAlert = true;
            _sensorTimer = 0;
            // refresh awareness if already aware
            if (_isAware) _awareTimer = _attentionTime;
            return true;
        }
        return false;
    }
    // ? executed twice first in behaviour tree then update tick
    protected void ProcessTrigger()
    {
        // is trigger allowed
        if (!_useTrigger || !_isSensors) return;
        float distance = float.MaxValue;
        foreach (SensorTrigger sensor in _sensorsTrigger)
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
                if (Vector3.Distance(Position, target.position) < distance)
                {
                    distance = Vector3.Distance(Position, target.position);
                    _positionTrigger = new EventPoint(target.position, target.gameObject.layer, Time.time);
                }
            }
            // print(gameObject.name + "\t" + sensor.Targets.Count);
        }
    }
    private bool CheckTrigger()
    {
        // is trigger allowed
        if (!_useTrigger || !_isSensors) return false;
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
    // // * testing snap
    // private void SetDirection(Vector3 target)
    // {
    //     _directionTarget = target;
    //     // 
    //     // Vector3 direction = (_directionTarget - Position).normalized;
    //     // float from = transform.eulerAngles.z % 360f;
    //     // from = from < 0 ? from + 360f : from;
    //     // float to = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f) % 360f;
    //     // to = to < 0 ? to + 360f : to;
    //     // _rotation = to - from;
    //     // _rotation = Mathf.Abs(_rotation) > 180 ? -_rotation % 180 : _rotation;
    //     // 
    //     // // * testing
    //     // transform.eulerAngles = new Vector3(0f, 0f, _rotation);
    //     // transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(target.y - Position.y, target.x - Position.x) * Mathf.Rad2Deg - 90f);
    // }
    private float _directionCheck;
    private Vector3 _directionTarget;
    // private bool CheckDirection(Vector3 target, float value)
    private bool CheckDirection()
    {
        Vector3 direction = (_directionTarget - Position).normalized;
        // float angle = (270f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // * testing initial angle should be zero
        if (direction.sqrMagnitude > 0f) angle -= 90f;
        angle = angle < 0 ? angle + 360f : angle;
        // print(angle + " : " + transform.eulerAngles.z);
        if (angle > transform.eulerAngles.z)
            return angle - transform.eulerAngles.z <= _directionCheck;
        return transform.eulerAngles.z - angle <= _directionCheck;
    }
    // * testing move
    void OnDrawGizmosSelected()
    {
        if (_searchType == TriggerSearchType.BOUNDS)
        {
            Gizmos.color = Color.red;
            // Gizmos.DrawWireCube(_motor.Position + (Vector3)_offset, new Vector3(_bounds.x, _bounds.y));
            // ? center on spawn once its been assigned
            Gizmos.DrawWireCube((HealthInst > 0 ? _spawn : Position) + (Vector3)_offset, new Vector3(_bounds.x - .1f, _bounds.y - .1f));
        }
        if (_searchType != TriggerSearchType.NULL)
        {
            // detected trigger sensors
            foreach (SensorTrigger sensor in _sensorsTrigger)
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
            // if (!_showPath) Gizmos.DrawLine(Position, _anchor.Position);
            // path
            // else if (_path != null)
            // {
            //     Gizmos.color = new Color(1f, 1f, 1f, .5f);
            //     // Vector3 current = Position;
            //     // foreach (Vector3 step in _path)
            //     // {
            //     //     Gizmos.DrawLine(current, step);
            //     //     current = step;
            //     // }
            //     // for (int i = _path.Length - 1; i > 0; i--)
            //     //     Gizmos.DrawLine(_path[i], _path[i-1]);
            //     Gizmos.DrawLine(Position, _anchor.Position);
            // }
            if (_showPath)
            {
                Gizmos.color = new Color(0f, 1f, 0f, .5f);
                Gizmos.DrawSphere(_path, .5f);
            }
        }
    }
    #region Navigation
    [SerializeField] private bool _showPath = false;
    // protected Vector3[] _path = null;
    // protected int _targetIndex = 0;
    // protected Vector3 _move;
    [SerializeField] protected int _timePath = 1;
    // for asyncronous movement
    protected int _timerPath = 0;
    protected float _speedMove = 0;
    protected Vector3 _spawn;
    [Tooltip("Can mob move diagonally")] [SerializeField] protected bool _isDiagonal = false;
    // [Tooltip("Initial state ? root (optional?)")] [SerializeField] protected Waypoint _waypoint;
    [Tooltip("Uses waypoint navigation")] [SerializeField] private bool _isNavigate = false;
    private Waypoint _waypoint;
    // ? rename to lookRotation moveRotation ? use instead bool check if turnSpeed zero for true
    public bool _testRotate = false;
    // * testing ? rename to...
    public void NavigateCancel()
    {
        // StopCoroutine("PathFollow");
        // _path = null;
        // _targetIndex = 0;
        // _move = Position;
        // 
        _path = Position;
    }
    // ? rename to get path
    public void NavigateTo(Vector3 target)
    // public void GetPathTo(Vector3 target)
    {
        if (GameGrid.Instance.WorldToIndex(target) != GameGrid.Instance.WorldToIndex(Position))
            GameNavigation.Instance.PathCalculate(new PathData(Position, target, _isDiagonal, OnPathFound));
    }
    public void OnPathFound(Vector3[] path, bool success)
    {
        if (success)
        {
            _path = path[0];
            // 
            // * testing mob walk "animation"
            Move(_path);
            // 
            // SetDirection();
            // 
            // print(_path.Length);
            // _path = path;
            // _targetIndex = 0;
            // StopCoroutine("PathFollow");
            // if (gameObject.activeSelf) StartCoroutine("PathFollow");
            // if (gameObject.activeSelf) PathFollow();
            // if (gameObject.activeSelf)
            // {
            //     // * testing
            //     // look in direction to move
            //     transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(path[0].y - Position.y, path[0].x - Position.x) * Mathf.Rad2Deg - 90f);
            //     // move forwards
            //     transform.position = path[0];
            //     // if (path.Length > 1) SetTarget(path[1]);
            //     // if (path.Length > 1) transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(path[1].y - Position.y, path[1].x - Position.x) * Mathf.Rad2Deg - 90f);
            // }
        }
    }
    private Vector3 _path;
    // IEnumerator PathFollow()
    // // private void PathFollow()
    // {
    //     // // * testing
    //     // // look in direction to move
    //     // if (!_testRotate) transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(_path[0].y - Position.y, _path[0].x - Position.x) * Mathf.Rad2Deg - 90f);
    //     // // move forwards
    //     // transform.position = _path[0];
    //     // 
    //     _move = _path[0];
    //     // * testing mob walk "animation"
    //     Move(_path[0]);
    //     yield return null;
    //     // 
    //     // if (_path.Length > 0)
    //     // {
    //     //     // _move = _path[0];
    //     //     // * testing
    //     //     // transform.position = _move;
    //     //     transform.position = _path[0];
    //     // }
    //     // while (true)
    //     // {
    //     //     if (Vector3.Distance(Position, _move) < 0.1f)
    //     //     {
    //     //         _targetIndex++;
    //     //         if (_path == null)
    //     //         {
    //     //             _targetIndex = 0;
    //     //             yield break;
    //     //         }
    //     //         else if (_targetIndex >= _path.Length)
    //     //         {
    //     //             _targetIndex = 0;
    //     //             _path = null;
    //     //             yield break;
    //     //         }
    //     //         _move = _path[_targetIndex];
    //     //     }
    //     //     yield return null;
    //     // }
    // }
    // // waypoint, else current position
    // protected Vector3 GetWaypoint()
    // {
    //     return _waypoint ? _waypoint.Position : _spawn;
    // }
    // waypoint, else current position
    protected Vector3 GetWaypoint(bool next, bool offset = false)
    {
        // if (_waypoint)
        // {
        //     _waypoint = _waypoint.GetNext();
        //     return _waypoint ? _waypoint.Position : _spawn;
        // }
        // return _spawn;
        // 
        if (next)
        {
            if (_waypoint) _waypoint = _waypoint.GetNext();
            else Debug.LogWarning(gameObject.name + ":\tAttempting to use waypoint without having one assigned first (check if isNavigate is enabled)");
        }
        return _waypoint ? (offset ? _waypoint.PositionRandom() : _waypoint.Position) : _spawn;
    }
    // // * testing
    // private void SetRotation(Vector3 target)
    // {
    //     // transform.eulerAngles.z = Mathf.ATan2((position.y - Position.y) / (position.x / Position.x)) * Mathf.Rad2Deg;
    //     // Vector3 direction = (target - _motor.Position).normalized;
    //     transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(target.y - Position.y, target.x - Position.x) * Mathf.Rad2Deg - 90f);
    // }
    private void SetRotation(float value)
    {
        // _rotation = value;
        // transform.eulerAngles = Vector3.forward * value;
        // 
        // * testing ? simulate eyes moving ? sprite not moved
        transform.eulerAngles = new Vector3(0f, 0f, -90f + value);
        // _body.eulerAngles = transform.eulerAngles;
        // transform.eulerAngles = new Vector3(0f, 0f, value);
        // _target = transform.position;
        // _rotationTo = -90f + value;
        // _rotation = -90f + value;
        // _target.position = _motor.Position;
        // _timer = _time;
        // _rotation = transform.eulerAngles.z;
        _rotation = 0f;
    }
    // 
    private void GetRotation()
    {
        Vector3 direction = (_directionTarget - Position).normalized;
        float from = transform.eulerAngles.z % 360f;
        from = from < 0 ? from + 360f : from;
        float to = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f) % 360f;
        // float to = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360f;
        // float to = (270f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360f;
        to = to < 0 ? to + 360f : to;
        _rotation = to - from;
        _rotation = Mathf.Abs(_rotation) > 180 ? -_rotation % 180 : _rotation;
    }
    #endregion
    #region Behaviour Tree
    // [Task]
    // void Testing(string value)
    // {
    //     print(value);
    //     ThisTask.Succeed();
    // }
    // [Task]
    // void AnchorToOffsetRadius(float value)
    // {
    //     // * testing [? retry attempts on invalid, chase bounds/limit]
    //     // _anchor.position += transform.right * Random.Range(-value, value) + transform.up * Random.Range(-value, value);
    //     // Vector3 direction = transform.right * Random.Range(-1f, 1f) + transform.up * Random.Range(-1f, 1f);
    //     // RaycastHit2D hit = Physics2D.Raycast(_anchor.Position, direction.normalized, value, game_variables.Instance.ScanLayerObstruction);
    //     // RaycastHit2D hit = Physics2D.Raycast(_motor.Position, direction.normalized, value, game_variables.Instance.ScanLayerObstruction);
    //     Vector2 offset = (transform.right * Random.Range(-1f, 1f) + transform.up * Random.Range(-1f, 1f)).normalized;
    //     RaycastHit2D hit = Physics2D.Raycast(Position, offset, value, GameVariables.ScanLayerObstruction);
    //     EventPoint temp = new EventPoint();
    //     // temp.Position = hit ? grid_map.Instance.WorldToGrid(hit.point + hit.normal * .3f) : grid_map.Instance.WorldToGrid(_motor.Position + (Vector3)offset * value);
    //     // temp.Position = hit ? grid_map.Instance.WorldToGrid(hit.point + hit.normal * .5f) : grid_map.Instance.WorldToGrid(_motor.Position + (Vector3)offset * value);
    //     // if (Physics2D.OverlapCircle(temp.Position, .9f, game_variables.Instance.ScanLayerObstruction) != null)
    //     //     temp.Position = grid_map.Instance.WorldToGrid(_motor.Position);
    //     // temp.Position = hit ? grid_map.Instance.WorldToGrid(_motor.Position) : grid_map.Instance.WorldToGrid(_motor.Position + (Vector3)offset * value);
    //     temp.Position = hit ? GameGrid.Instance.WorldToGrid(hit.point + hit.normal * .5f) : GameGrid.Instance.WorldToGrid(Position);
    //     // safety ?
    //     if (Physics2D.OverlapCircle(temp.Position, .9f, GameVariables.ScanLayerObstruction) != null)
    //         // temp.Position = grid_map.Instance.WorldToGrid(_motor.Position);
    //         temp.Position = GetWaypoint();
    //     temp.Layer = _anchor.Layer;
    //     temp.Time = _anchor.Time;
    //     _anchor = temp;
    //     // else
    //     //     // temp.Position = _anchor.Position + direction;
    //     //     // temp.Position = _motor.Position + direction.normalized * value;
    //     //     temp.Position = _anchor.Position;
    //     // // ? locking
    //     // while (hit)
    //     // {
    //     //     direction = transform.right * Random.Range(-value, value) + transform.up * Random.Range(-value, value);
    //     //     hit = Physics2D.Raycast(_anchor.Position, direction.normalized, value, game_variables.Instance.ScanLayerObstruction);
    //     // }
    //     // temp.Position = _anchor.Position + direction;
    //     Task.current.Succeed();
    // }
    [Task]
    // void AnchorToBack(float max, float min)
    void AnchorToDirection(int dir, float max, float min)
    {
        // clockwise from 0 to 3
        Vector3 direction = Vector3.up;
        if (dir == 1) direction = Vector3.left;
        else if (dir == 2) direction = Vector3.down;
        else if (dir == 3) direction = Vector3.right;
        // global to local
        direction = transform.TransformDirection(direction);
        // do occlusion check in straight line behind creature
        RaycastHit2D hit = Physics2D.Raycast(Position, direction, max, GameVariables.ScanLayerObstruction);
        // all clear
        if (!hit)
        {
            // mark position for movement
            _anchor = new EventPoint(Position + direction * max, _anchor.Layer, _anchor.Time);
            // found valid position
            ThisTask.Succeed();
        }
        // acceptable distance
        else if (hit.distance >= min)
        {
            // mark position for movement
            _anchor = new EventPoint(hit.point + (Vector2)direction * .1f, _anchor.Layer, _anchor.Time);
            // found valid position
            ThisTask.Succeed();
        }
        // no place to move to
        else ThisTask.Fail();
    }
    [Task]
    void AnchorToTrigger()
    {
        // nothing detected
        if (_positionTrigger == null) ThisTask.Fail();
        else
        {
            ThisTask.debugInfo = _positionTrigger.Position.ToString();
            _anchor = _positionTrigger;
            // ..? why did do this
            // _positionTrigger = null;
            ThisTask.Succeed();
        }
    }
    // * testing
    [Task]
    void AnchorToVision()
    {
        // nothing detected
        if (_positionVision == null) ThisTask.Fail();
        else
        {
            ThisTask.debugInfo = _positionVision.Position.ToString();
            _anchor = _positionVision;
            // ..? why did do this
            // _positionVision = null;
            ThisTask.Succeed();
        }
    }
    [Task]
    void AnchorToWaypoint(int value)
    {
        // 0 - current waypoint | 1 - next waypoint
        _anchor = new EventPoint(GetWaypoint(value == 1), _anchor.Layer, _anchor.Time);
        ThisTask.Succeed();
    }
    [Task]
    void AnchorToWaypointOffset()
    {
        // 0 - current waypoint | 1 - next waypoint
        _anchor = new EventPoint(GetWaypoint(true, true), _anchor.Layer, _anchor.Time);
        Task.current.Succeed();
    }
    // [Task]
    // void DoAttack(int indexPrefab)
    // {
    //     // ? not needed anymore
    //     NavigateCancel();
    //     // 
    //     GameObject temp = Instantiate(_prefabs[indexPrefab], GameVariables.PositionDamage(_anchor.Position), transform.rotation);
    //     // 
    //     temp.GetComponent<BaseHitbox>().Initialize(this as Breakable, _anchor.Position);
    //     // 
    //     temp.SetActive(true);
    //     // 
    //     ThisTask.Succeed();
    // }
    [Task]
    void DoAttack(int indexPrefab, int indexSpawn, int parent)
    {
        // // * testing [? safety]
        // _anim.SetTarget(_anchor.Position);
        NavigateCancel();
        // ? value not returned
        GameObject temp = Instantiate(_prefabs[indexPrefab], GameVariables.PositionDamage(_spawns[indexSpawn].position), _spawns[indexSpawn].rotation);
        // Entity temp = ManagerPool.PooledGet("Attack/" + _attackPooled[indexPrefab], GameVariables.PositionDamage(_spawns[indexSpawn].position), _spawns[indexSpawn].rotation);
        // ManagerPool.PooledGet("Attack/" + _attackPooled[indexPrefab], GameVariables.PositionDamage(_spawns[indexSpawn].position), _spawns[indexSpawn].rotation);
        // print(temp);
        // 
        temp.GetComponent<BaseHitbox>().Initialize(this as Breakable, _anchor.Position);
        // (temp as BaseHitbox).Initialize(this as Breakable, _anchor.Position);
        // 
        if (parent == 1) temp.transform.SetParent(_spawns[indexSpawn]);
        // 
        temp.SetActive(true);
        // temp.ToggleActive(true);
        // 
        ThisTask.Succeed();
    }
    [Task]
    void EntityAtAnchor(float value)
    {
        // ignore z depth
        float distance = Vector2.Distance(Position, _anchor.Position);
        ThisTask.debugInfo = distance.ToString();
        ThisTask.Complete(distance <= value);
    }
    [Task]
    void EntityToAnchor()
    {
        ThisTask.debugInfo = _anchor.Position.ToString();
        // if (_timerPath <= 0 && _speedMove != 0)
        // {
        //     // * testing asynchronous
        //     _timerPath = _timePath + Random.Range(-.2f, .2f);
        //     NavigateTo(_anchor.Position);
        // }
        // ? change to bool isMove
        if (_speedMove > 0) NavigateTo(_anchor.Position);
        // if (_speedMove > 0) GetPathTo(_anchor.Position);
        // 
        // Move(_anchor.Position);
        // 
        // ThisTask.Succeed();
        // ? promote to property
        ThisTask.Complete(_speedMove > 0);
    }
    // [Task]
    // void EntityToPath()
    // {
    //     ThisTask.debugInfo = (_path - Position).ToString();
    //     // 
    //     Move(_path);
    //     // 
    //     ThisTask.Succeed();
    // }
    // [Task]
    // void FindPathToAnchor()
    // {
    //     // ? warning on speed check fail
    //     if (_speedMove > 0) GetPathTo(_anchor.Position);
    //     ThisTask.debugInfo = _path.ToString();
    //     ThisTask.Succeed();
    // }
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
    void IsAware()
    {
        // Task.current.debugInfo = (_awareTimer / _attentionTime) + " : 0";
        ThisTask.debugInfo = _awareTimer + " : " + _attentionTime;
        ThisTask.Complete(_isAware);
    }
    [Task]
    void IsBored()
    {
        ThisTask.debugInfo = _sensorTimer + " : " + _attentionTime;
        ThisTask.Complete(_sensorTimer == _attentionTime);
    }
    [Task]
    void IsDirection(float value)
    // void IsDirection()
    {
        // // * testing [? safety]
        // NavigateCancel();
        // SetTarget(_anchor.Position);
        // SetRotation(_anchor.Position);
        // GetRotation();
        ThisTask.debugInfo = _rotation.ToString();
        // * testing
        _directionCheck = value;
        _directionTarget = _anchor.Position;
        ThisTask.Complete(CheckDirection());
    }
    [Task]
    void IsAnchorCreature()
    {
        ThisTask.debugInfo = _anchor.Position.ToString();
        Collider2D hit = Physics2D.OverlapCircle(_anchor.Position, .1f, GameVariables.ScanLayerCreature);
        ThisTask.Complete(hit && _hostiles.Contains(hit.transform.GetComponent<Entity>().ID));
    }
    [Task]
    void IsAnchorWaypoint()
    {
        ThisTask.Complete(_waypoint ? _waypoint.IsWaypoint(_anchor.Position) : false);
    }
    [Task]
    void IsFlag(int index)
    {
        // * testing [? check flag int default zero]
        if (index > -1 && index < _countFlags)
        {
            ThisTask.debugInfo = _flags[index].ToString();
            ThisTask.Complete(_flags[index]);
        }
        else
        {
            Debug.LogWarning(gameObject.name + ":\tAttempting to access undefined flag");
            // 
            ThisTask.Fail();
        }
    }
    [Task]
    void IsTimer(int index)
    {
        if (index > -1 && index < _countTimers)
        {
            ThisTask.debugInfo = _timers[index].ToString();
            ThisTask.Complete(_timers[index] > 0);
        }
        else
        {
            Debug.LogWarning(gameObject.name + ":\tAttempting to access undefined timer");
            // 
            ThisTask.Succeed();
        }
    }
    // * testing [? use entity timer]
    protected int _scanTimer = 0;
    // protected float[] _scanOffset;
    protected float _scanOffset;
    // * testing ? game variables or utility
    private AnimationCurve _lerp;
    // * testing ? sensor interrupt, animation rotation offset?
    // angle - deviation from forward to rotate in
    // duration - time taken to complete one full scan
    // steps - number of scans
    [Task]
    void ScanVision(float angle, int duration, int steps)
    {
        // * testing
        NavigateCancel();
        Task.current.debugInfo = _scanTimer.ToString();
        // if (_scanTimer >= 0f)
        if (_scanTimer >= 0)
        {
            // SetRotation(_scanOffset + angle * Mathf.Sin(((_scanTimer % step) / step) * Mathf.PI * 2f));
            // SetRotation(_scanOffset + angle * Mathf.Sin(((_scanTimer % duration) / (float)duration) * Mathf.PI * 2f));
            SetRotation(_scanOffset + angle * _lerp.Evaluate((_scanTimer % duration) / (float)duration));
            // print(Mathf.Sin(((_scanTimer % duration) / duration) * Mathf.PI * 2f));
            // print(_scanTimer + "\t" + (1f - ((_scanTimer % step) / step)).ToString());
            // print((int)_scanTimer % duration);
            // print(Mathf.Lerp(-angle, angle, (_scanTimer % duration) / (float)duration));
            // _scanTimer -= Time.deltaTime;
            _scanTimer--;
            // // * testing ? approximate
            // _scanTimer -= 1f;
            // if (_scanTimer < 0f)
            if (_scanTimer < 0)
            {
                SetRotation(_scanOffset);
                Task.current.Succeed();
                return;
            }
        }
        else
        {
            // * testing [? memory waste]
            // _scanTimer = duration;
            _scanTimer = duration * steps;
            _scanOffset = 90f + _rotation;
            // _scanOffset = _rotation;
        }
        Task.current.Fail();
    }
    // ? redundant, isAlert holds result of same operation, but needs manual reset?
    [Task]
    void SensorAny()
    {
        ThisTask.debugInfo = "S:" + _isSensors + " V:" + _useVision + " T:" + _useTrigger;
        // check sensors [priority]
        ThisTask.Complete(CheckTrigger() || CheckVision());
    }
    [Task]
    void SetAttack(int index, int value)
    {
        if (_prefabs[index].activeSelf != (value == 1))
        {
            _prefabs[index].GetComponent<BaseHitbox>().Initialize(this as Breakable, _anchor.Position);
            _prefabs[index].SetActive(value == 1);
        }
        ThisTask.Succeed();
    }
    [Task]
    void SetColor(int index)
    {
        if (index > -1 && index < _colors.Length)
        {
            _sprite.color = _colors[index];
            ThisTask.Succeed();
        }
        else
        {
            Debug.LogWarning(gameObject.name + ":\tAttempting to use undefined color");
            // 
            ThisTask.Fail();
        }
    }
    [Task]
    void SetColliders(int value)
    {
        SetColliders(value == 1);
        // _motor.CollidersToggle(value == 1);
        ThisTask.Succeed();
    }
    // // * testing
    // [Task]
    // void SetDirectionCheck(float value)
    // {
    //     _directionCheck = value;
    //     ThisTask.Succeed();
    // }
    // [Task]
    // void SetRotationOffset(float value)
    // {
    //     _rotation = transform.eulerAngles.z + value;
    // }
    [Task]
    void SetFlag(int index, int value)
    {
        if (index > -1 && index < _countFlags)
        {
            _flags[index] = value == 1;
            ThisTask.Succeed();
        }
        else
        {
            Debug.LogWarning(gameObject.name + ":\tAttempting to use undefined flag");
            // 
            ThisTask.Fail();
        }
    }
    // ? rename to set speed move for consistency
    [Task]
    void SetSpeedMove(float value)
    {
        if (value == 0 && _speedMove != 0) NavigateCancel();
        _speedMove = value;
        ThisTask.Succeed();
    }
    // * testing
    [Task]
    void SetSpeedTurn(float value)
    {
        _speedTurn = value;
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
        else
        {
            Debug.LogWarning(gameObject.name + ":\tAttempting to use undefined sprite");
            // 
            ThisTask.Fail();
        }
    }
    // // look target
    // // 0 - anchor [sensor detection implicit]
    // // 1 - path
    // [Task]
    // void SetTarget(int value)
    // {
    //     // SetDirection(value == 0 ? _anchor.Position : _path);
    //     _directionTarget = value == 0 ? _anchor.Position : _path;
    //     // ThisTask.debugInfo = _directionTarget.ToString();
    //     ThisTask.Succeed();
    // }
    [Task]
    void SetTimer(int index, int value)
    {
        if (index > -1 && index < _countTimers)
        {
            _timers[index] = value;
            ThisTask.Succeed();
        }
        else
        {
            Debug.LogWarning(gameObject.name + ":\tAttempting to use undefined timer");
            // 
            ThisTask.Fail();
        }
    }
    [Task]
    void SetTriggers(int value)
    {
        bool status = value == 1;
        // not already active/inactive
        if (_useTrigger != status)
        {
            // disable/enable all trigger sensors
            // foreach(SensorTrigger sensor in _sensorsTrigger) sensor.gameObject.SetActive(status);
            foreach(SensorTrigger sensor in _sensorsTrigger) sensor.ToggleActive(status);
                // if (status) sensor.Show();
                // else sensor.Hide();
            // ? only bool check disable works fine since sensor not shown visually, can use for example hiding roots when seedFlower dies? doesnt work for shroomPrisoner, saves extra calculation
            _useTrigger = status;
        }
        ThisTask.Succeed();
    }
    [Task]
    void SetVisions(int value)
    {
        bool status = value == 1;
        // not already active/inactive
        if (_useVision != status)
        {
            // disable/enable all vision sensors
            // foreach(SensorVision sensor in _sensorsVision) sensor.gameObject.SetActive(status);
            foreach(SensorVision sensor in _sensorsVision) sensor.ToggleActive(status);
                // if (status) sensor.Show();
                // else sensor.Hide();
            // 
            _useVision = status;
        }
        ThisTask.Succeed();
    }
    [Task]
    void TrackAnchor()
    {
        // check signed angle between forward and anchor
        float angle = Vector2.SignedAngle(transform.up, _anchor.Position - Position);
        // X, facing
        if (angle >= -45f && angle <= 45f) ThisTask.Succeed();
        // Y, left
        else if (angle > 45f)
        {
            transform.eulerAngles += Vector3.forward * 90f;
            _body.eulerAngles = transform.eulerAngles;
            // 
            ThisTask.Fail();
        }
        // -Y, right
        // else if (angle < -45f)
        else
        {
            transform.eulerAngles -= Vector3.forward * 90f;
            _body.eulerAngles = transform.eulerAngles;
            // 
            ThisTask.Fail();
        }
    }
    [Task]
    void UnsetAlert()
    {
        _isAlert = false;
        ThisTask.Succeed();
    }
    #endregion
}