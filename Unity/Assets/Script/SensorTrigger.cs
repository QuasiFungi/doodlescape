using UnityEngine;
using System.Collections.Generic;
// ? box collider
[RequireComponent(typeof(CircleCollider2D))]
public class SensorTrigger : Entity
{
    protected List<Transform> _targets;
    protected SpriteRenderer _sprite;
    // 
    // #region RENDER FOV
    // public float _resolution = 1f;
    // private MeshFilter viewMeshFilter;
    // private Mesh viewMesh;
    // private float Angle;
    public float Radius = 0.4f;
    // [Tooltip("visually show area covered by sensor")] public bool _isRender = false;
    void Awake()
    {
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
        _sprite = GetComponent<SpriteRenderer>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = Radius;
        // GetComponent<CircleCollider2D>().isTrigger = true;
        // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        // print(other.gameObject.name);
        // ignore repeat, non creature/breakable, trigger colliders
        if (_targets.Contains(other.transform) || (other.gameObject.layer != GameVariables.LayerCreature && other.gameObject.layer != GameVariables.LayerBreakable) || other.isTrigger) return;
        // // target visible ? makes flyTrap not respond to slab, use 360 vision for visiblity check
        // if (Physics2D.Raycast(transform.position, (other.transform.position - transform.position).normalized, Vector3.Distance(transform.position, other.transform.position), GameVariables.ScanLayerObstruction)) return;
        // exclude dead new
        if (other.gameObject.layer == GameVariables.LayerCreature)
        {
            // // * testing invisible
            // if (other.gameObject.layer == game_variables.Instance.LayerPlayer && other.GetComponent<data_player>().ModeInvisible)
            //     return;
            if (other.GetComponent<Creature>().HealthInst > 0)
                _targets.Add(other.transform);
        }
        else _targets.Add(other.transform);
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (_targets.Contains(other.transform))
            _targets.Remove(other.transform);
    }
    public List<Transform> Targets
    {
        get { return _targets; }
    }
}