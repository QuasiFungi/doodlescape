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
    private void BehaviourTick()
    {
        // get latest info
        UpdateState();
        // act on new info
        _ai.Tick();
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
    [Tooltip("colors this entity can take")] [SerializeField] protected Color[] _colors;
    [Tooltip("Number of internal timers")] [SerializeField] protected int _countTimers;
    protected float[] _timers;
    [Tooltip("Number of internal flags")] [SerializeField] protected int _countFlags;
    protected bool[] _flags;
    // * testing [attack ? effect]
    [Tooltip("Attacks, effects, etc")] [SerializeField] protected GameObject[] _prefabs;
    [Tooltip("Prefab spawn points")] [SerializeField] protected Transform[] _spawns;
    // * testing load sensors trigger
    [Tooltip("Sensor trigger gameObject")] [SerializeField] protected string _trigger = "";
    [Tooltip("Sensors check area")] [SerializeField] protected Vector2Int _bounds = Vector2Int.one;
    [Tooltip("Check area offset")] [SerializeField] protected Vector2 _offset = Vector2.zero;
    // for asyncronous movement
    protected float _timerPath;
    // ? use string ids
    [Tooltip("creatures that will be attacked")] [SerializeField] protected List<Transform> _hostiles;
    // ? taken from anim or entity
    protected SpriteRenderer _sprite;
    [Tooltip("Mob sprite variants (0 is default)")] [SerializeField] protected Sprite[] _sprites;
    protected List<Collider2D> _colliders;
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
        // ? move to anim
        _sprite = GetComponent<SpriteRenderer>();
        // * testing ? move to motor
        _colliders = new List<Collider2D>();
        _colliders.Add(GetComponent<Collider2D>());
        // 
        if (_trigger != "")
            for (float x = -_bounds.x / 2; x <= _bounds.x / 2; x++)
                for (float y = -_bounds.y / 2; y <= _bounds.y / 2; y++)
                {
                    // // * testing
                    // _test.Add((Vector2)transform.position + new Vector2(x, y) + _offset);
                    // sensor_trigger temp = Physics2D.OverlapCircle(_test[_test.Count - 1], .9f, game_variables.Instance.ScanLayerSensor)?.gameObject.GetComponent<sensor_trigger>();
                    TestTrigger temp = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(x, y) + _offset, .1f, GameVariables.ScanLayerSensor)?.gameObject.GetComponent<TestTrigger>();
                    if (temp?.name == _trigger && !_sensorsTrigger.Contains(temp))
                        _sensorsTrigger.Add(temp);
                }
        foreach (TestTrigger sensor in _sensorsTrigger)
            sensor.SetActive(true);
    }
    void Update()
    {
        // ? memory waste, assume time passed is 1 second since last updatestate
        if (ProcessVision() || ProcessTrigger()) _awareTimer += _attentionTime * Time.deltaTime;
        // 
        else if (_awareTimer > 0) _awareTimer -= Time.deltaTime;
        // ? check if not already max
        _sensorTimer = Mathf.Clamp(_sensorTimer + Time.deltaTime, 0, _attentionTime);
        // 
        for (int i = 0; i < _countTimers; i++) if (_timers[i] > 0f) _timers[i] -= Time.deltaTime;
        // 
        if (_timerPath > 0) _timerPath -= Time.deltaTime;
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
                if (!_hostiles.Contains(target))
                    continue;
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
                if (!_hostiles.Contains(target))
                    continue;
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
        // GameObject temp = Instantiate(_prefabs[indexPrefab], _spawns[indexSpawn].position, _spawns[indexSpawn].rotation);
        // temp.GetComponent<base_hitbox>().Initialize(transform);
        // if (parent == 1)
        //     temp.transform.SetParent(_spawns[indexSpawn]);
        // temp.SetActive(true);
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