using UnityEngine;
// using UnityEngine;
using System.Collections.Generic;
// ? box collider
// [RequireComponent(typeof(CircleCollider2D))]
public class SensorTrigger : Entity
{
    protected List<Transform> _targets;
    // protected SpriteRenderer _sprite;
    // 
    // #region RENDER FOV
    // public float _resolution = 1f;
    // private MeshFilter viewMeshFilter;
    // private Mesh viewMesh;
    // private float Angle;
    [Header("Trigger")]
    [SerializeField] [Range(0, 6)] private int _radius = 0;
    // public float Radius = 0.4f;
    // [Tooltip("visually show area covered by sensor")] public bool _isRender = false;
    // * testing room transition
    [Tooltip("Default active state")] [SerializeField] private bool _default = true;
    private SpriteRenderer _renderer;
    protected override void Awake()
    {
        // does not have loot
        base.Awake();
        // 
        // if (_isRender)
        // {
        //     viewMeshFilter = GetComponent<MeshFilter>();
        //     viewMesh = new Mesh();
        //     viewMesh.name = "View Mesh";
        //     viewMeshFilter.mesh = viewMesh;
        //     Angle = 360f;
        // }
        // 
        _targets = new List<Transform>();
        // _sprite = GetComponent<SpriteRenderer>();
        // CircleCollider2D collider = GetComponent<CircleCollider2D>();
        // collider.isTrigger = true;
        // collider.radius = Radius;
        // GetComponent<CircleCollider2D>().isTrigger = true;
        // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        // 
        _renderer = transform.GetComponent<SpriteRenderer>();
        SetColor(false);
    }
    // * testing room transition, restore default state on enable
    [Tooltip("Active state controlled by chunk only")] [SerializeField] private bool _isRestricted = false;
    // public override void ToggleActive(bool state, bool isChunk = false)
    // {
    //     if (_isRestricted && !isChunk) return;
    //     // 
    //     base.ToggleActive(state, isChunk);
    // }
    public void ToggleDisabled(bool state)
    {
        if (_isRestricted) return;
        // 
        ToggleActive(state ? false : _default);
    }
    // protected override void OnEnable()
    // {
    //     base.OnEnable();
    //     // 
    //     Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, Radius, GameVariables.ScanLayerTarget);
    //     for (int i = targets.Length - 1; i > -1; i--) OnTriggerEnter2D(targets[i]);
    //     if (_isDebug) print("Enabled: " + _targets.Count);
    // }
    // protected override void OnDisable()
    // {
    //     base.OnDisable();
    //     // forget all trespassers
    //     for (int i = _targets.Count - 1; i > -1; i--) OnTriggerExit2D(_targets[i].GetComponent<Collider2D>());
    //     if (_isDebug) print("Disabled: " + _targets.Count);
    // }
    protected override void OnEnable()
    {
        // base.OnEnable();
        // // 
        // // ? possible bug with tick timing
        // StartCoroutine("FindTargetsWithDelay", 0.2f);
        if (_isRestricted) return;
        GameClock.onTick += FindTargets;
        GameClock.onTickUI += FindTargets;
        // 
        // no need since data not tracked
        // base.OnEnable();
    }
    protected override void OnDisable()
    {
        // base.OnDisable()
        // // 
        // skip calculations if AI off
        // StopCoroutine("FindTargetsWithDelay");
        if (_isRestricted) return;
        GameClock.onTick -= FindTargets;
        GameClock.onTickUI -= FindTargets;
        // // show detection state
        // SetColor(false);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = GameVariables.ColorSensor;
        Gizmos.DrawWireSphere(Position, Radius);
    }
    // public bool _isDebug = false;
    [Tooltip("Detect Breakable entities")] [SerializeField] private bool _isBreakable = false;
    void FindTargets()
    {
        // forget all trespassers
        _targets.Clear();
        // get all entities in radius
        Collider2D[] targets = Physics2D.OverlapCircleAll(Position, Radius, GameVariables.ScanLayerTrigger);
        // if (_isDebug) print(targets.Length);
        // iterate all targets
        for (int i = targets.Length - 1; i > -1; i--)
        {
            // if (_isDebug) print(targets[i].name);
            // ignore repeat, non creature/breakable, trigger colliders
            if (_targets.Contains(targets[i].transform) || (targets[i].gameObject.layer != GameVariables.LayerCreature && _isBreakable && targets[i].gameObject.layer != GameVariables.LayerBreakable) || targets[i].isTrigger) continue;
            // exclude dead new
            if (targets[i].gameObject.layer == GameVariables.LayerCreature)
            {
                if (targets[i].GetComponent<Creature>().HealthInst > 0) _targets.Add(targets[i].transform);
            }
            else _targets.Add(targets[i].transform);
        }
        // show detection state ? bugged with delayed mob movement calculation
        SetColor(_targets.Count > 0);
    }
    // // ? move to base sensor class
    // public Vector3 DirectionFromAngle(float angle, bool isGlobal)
    // {
    //     if (!isGlobal) angle -= transform.eulerAngles.z;
    //     return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
    // }
    // void DrawFOV()
    // {
    //     int stepCount = Mathf.RoundToInt(Angle * _resolution);
    //     float stepAngleSize = Angle / stepCount;
    //     List<Vector3> viewPoints = new List<Vector3>();
    //     for (int i = 0; i <= stepCount; i++)
    //     {
    //         float angle = transform.eulerAngles.z - Angle / 2 + stepAngleSize * i + 90f;
    //         // ViewCastInfo newViewCast = ViewCast(angle);
    //         // viewPoints.Add(newViewCast.point);
    //         viewPoints.Add(transform.position + DirectionFromAngle(angle, true) * Radius);
    //     }
    //     int vertexCount = viewPoints.Count + 1;
    //     Vector3[] vertices = new Vector3[vertexCount];
    //     int[] triangles = new int[(vertexCount - 2) * 3];
    //     vertices[0] = Vector3.zero;
    //     for (int i = 0; i < vertexCount - 1; i++)
    //     {
    //         vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
    //         if (i < vertexCount - 2)
    //         {
    //             triangles[i * 3] = 0;
    //             triangles[i * 3 + 1] = i + 1;
    //             triangles[i * 3 + 2] = i + 2;
    //         }
    //     }
    //     viewMesh.Clear();
    //     viewMesh.vertices = vertices;
    //     viewMesh.triangles = triangles;
    //     viewMesh.RecalculateNormals();
    // }
    // void LateUpdate()
    // {
    //     if (_isRender) DrawFOV();
    // }
    // #endregion
    // void Update()
    // {
    //     // * testing
    //     if (_sprite)
    //         // _sprite.enabled = Vector3.Distance(transform.position, controller_player.Instance.Motor.Position) <= game_variables.Instance.RadiusSprite;
    //         _sprite.enabled = game_camera.Instance.InView(transform.position);
    // }
    // public void SetActive(bool value)
    // {
    //     if (gameObject.activeSelf != value)
    //         gameObject.SetActive(value);
    // }
    // [Tooltip("Name each gameObject that touches collider")] public bool _isDebug = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // // ignore repeat, non creature/breakable, trigger colliders
        // if (_targets.Contains(other.transform) || (other.gameObject.layer != GameVariables.LayerCreature && other.gameObject.layer != GameVariables.LayerBreakable) || other.isTrigger) return;
        // // * testing snail not detecting player
        // if (_isDebug) print("Enter: " + other.gameObject.name);
        // // // target visible ? makes flyTrap not respond to slab, use 360 vision for visiblity check
        // // if (Physics2D.Raycast(transform.position, (other.transform.position - transform.position).normalized, Vector3.Distance(transform.position, other.transform.position), GameVariables.ScanLayerObstruction)) return;
        // // exclude dead new
        // if (other.gameObject.layer == GameVariables.LayerCreature)
        // {
        //     // // * testing invisible
        //     // if (other.gameObject.layer == game_variables.Instance.LayerPlayer && other.GetComponent<data_player>().ModeInvisible)
        //     //     return;
        //     if (other.GetComponent<Creature>().HealthInst > 0)
        //     {
        //         _targets.Add(other.transform);
        //         // * testing sfx crunch
        //         // if (_isRestricted && other.gameObject.tag == "player") GameAudio.Instance.Register(2, GameAudio.AudioType.ENTITY);
        //         if (_isRestricted) GameAudio.Instance.Register(2, GameAudio.AudioType.ENTITY);
        //     }
        // }
        // else
        // {
        //     _targets.Add(other.transform);
        //     // // * testing sfx crunch
        //     // GameAudio.Instance.Register(3);
        // }
        // // 
        // SetColor(_targets.Count > 0);
        // * testing sfx crunch
        if (_isRestricted)
        {
            GameAudio.Instance.Register(2, GameAudio.AudioType.ENTITY);
            FindTargets();
        }
    }
    private bool _isDetected = true;
    private void SetColor(bool isDetected)
    {
        // unchanged unavailable, ignore
        if (_renderer == null || _isDetected == isDetected) return;
        // 
        _isDetected = isDetected;
        // ? flash vs persistent
        _renderer.color = _isDetected ? GameVariables.ColorDefault : GameVariables.ColorSensor;
    }
    private void OnTriggerExit2D(Collider2D other) {
        // if (_targets.Contains(other.transform)) _targets.Remove(other.transform);
        // // 
        // if (_isDebug) print("Exit: " + other.gameObject.name);
        // 
        // SetColor(_targets.Count > 0);
        // 
        if (_isRestricted) FindTargets();
    }
    public List<Transform> Targets
    {
        get { return _targets; }
    }
    private float Radius
    {
        get { return _radius + .4f; }
    }
}