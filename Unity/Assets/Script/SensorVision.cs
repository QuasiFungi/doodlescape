using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SensorVision : Entity
{
    public float Radius = 5f;
    [Range(0, 360)] public float Angle = 90f;
    // ? store creature class references
    protected List<Transform> _targets = new List<Transform>();
    // 
    #region RENDER FOV
    public float _resolution = 1f;
    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    void Awake()
    {
        viewMeshFilter = GetComponent<MeshFilter>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;
        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }
    public Vector3 DirectionFromAngle(float angle, bool isGlobal)
    {
        if (!isGlobal) angle -= transform.eulerAngles.z;
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
    }
    ViewCastInfo ViewCast(float angleGlobal)
    {
        Vector3 direction = DirectionFromAngle(angleGlobal, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Radius, GameVariables.ScanLayerObstruction);
        if (hit) return new ViewCastInfo(true, new Vector3(hit.point.x, hit.point.y, transform.position.z), hit.distance, angleGlobal);
        return new ViewCastInfo(false, transform.position + direction * Radius, Radius, angleGlobal);
    }
    void DrawFOV()
    {
        int stepCount = Mathf.RoundToInt(Angle * _resolution);
        float stepAngleSize = Angle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - Angle / 2 + stepAngleSize * i + 90f;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }
    void LateUpdate()
    {
        DrawFOV();
    }
    #endregion
    // 
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * Radius);
        // ! borked
        // Gizmos.color = Color.magenta;
        // Gizmos.DrawLine(transform.position, transform.position + new Vector3(Radius * Mathf.Cos(Angle / 2), Radius * Mathf.Sin(Angle / 2), 0f));
        // Gizmos.DrawLine(transform.position, transform.position + new Vector3(-Radius * Mathf.Cos(-Angle / 2), -Radius * Mathf.Sin(-Angle / 2), 0f));
        // Gizmos.DrawWireSphere(transform.position, Radius);
        // 
        // if (_testing)
        // {
        //     Gizmos.color = Color.red;
        //     foreach (Transform target in _testTargets) Gizmos.DrawLine(transform.position, target.position);
        //     foreach (Transform target in _targets) Gizmos.DrawSphere(target.position, .1f);
        // }
    }
    // called on parent active toggle ?
    void OnEnable()
    {
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }
    void OnDisable()
    {
        StopCoroutine("FindTargetsWithDelay");
    }
    // public void SetActive(bool value)
    // {
    //     if (gameObject.activeSelf != value)
    //         gameObject.SetActive(value);
    // }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }
    // public bool _testing = false;
    // private List<Transform> _testTargets = new List<Transform>();
    void FindTargets()
    {
        _targets.Clear();
        // _testTargets.Clear();
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, Radius, GameVariables.ScanLayerTarget);
        for (int i = 0; i < targets.Length; i++)
        {
            // exclude trigger
            if (targets[i].isTrigger)
                continue;
            Transform target = targets[i].transform;
            if (target.position == transform.position)
                continue;
            // // * testing invisible
            // if (target.gameObject.layer == game_variables.Instance.LayerPlayer && target.GetComponent<data_player>().ModeInvisible)
            //     continue;
            // 
            // _testTargets.Add(target);
            // 
            // exclude dead
            if (target.gameObject.layer == GameVariables.LayerCreature)
                if (target.GetComponent<Creature>().HealthInst == 0)
                    continue;
            // ? use vector2 here
            Vector3 direction = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.up, direction) <= Angle / 2f)
            {
                float distance = Vector2.Distance(transform.position, target.position);
                // center left right ? memory overhead
                if (!Physics2D.Raycast(transform.position, direction, distance, GameVariables.ScanLayerObstruction)
                    || !Physics2D.Raycast(transform.position + Vector3.Cross(direction, Vector3.forward) * .1f, direction, distance, GameVariables.ScanLayerObstruction)
                    || !Physics2D.Raycast(transform.position + Vector3.Cross(Vector3.forward, direction) * .1f, direction, distance, GameVariables.ScanLayerObstruction))
                    // player mob item
                    _targets.Add(target);
                // else if(_testing) print(target.name + ": failed raycast check");
            }
            // else if(_testing) print(target.name + ": failed angle check");
        }
    }
    public Vector3 Position
    {
        get { return transform.position; }
    }
    public List<Transform> Targets
    {
        get { return _targets; }
    }
    public float Rotation
    {
        get { return transform.eulerAngles.z; }
        set { transform.eulerAngles = Vector3.forward * value; }
    }
}