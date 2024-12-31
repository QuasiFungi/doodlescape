// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// public class TestVision : MonoBehaviour
// {
//     public float Radius = 5f;
//     [Range(0, 360)] public float Angle = 90f;
//     // ? store creature class references
//     protected List<Transform> _targets = new List<Transform>();
//     // #region RENDER FOV
//     // public float _resolution = 1f;
//     // private MeshFilter viewMeshFilter;
//     // private Mesh viewMesh;
//     // void Awake()
//     // {
//     //     viewMeshFilter = GetComponent<MeshFilter>();
//     //     viewMesh = new Mesh();
//     //     viewMesh.name = "View Mesh";
//     //     viewMeshFilter.mesh = viewMesh;
//     //     // StartCoroutine("FindTargetsWithDelay", 0.2f);
//     // }
//     // public struct ViewCastInfo
//     // {
//     //     public bool hit;
//     //     public Vector3 point;
//     //     public float distance;
//     //     public float angle;
//     //     public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
//     //     {
//     //         this.hit = hit;
//     //         this.point = point;
//     //         this.distance = distance;
//     //         this.angle = angle;
//     //     }
//     // }
//     // public Vector3 DirectionFromAngle(float angle, bool isGlobal)
//     // {
//     //     if (!isGlobal)
//     //         angle -= transform.eulerAngles.z;
//     //     return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0f);
//     // }
//     // ViewCastInfo ViewCast(float angleGlobal)
//     // {
//     //     Vector3 direction = DirectionFromAngle(angleGlobal, true);
//     //     RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Radius, GameVariables.ScanLayerSolid);
//     //     if (hit)
//     //     {
//     //         return new ViewCastInfo(true, hit.point, hit.distance, angleGlobal);
//     //     }
//     //     return new ViewCastInfo(false, transform.position + direction * Radius, Radius, angleGlobal);
//     // }
//     // void DrawFOV()
//     // {
//     //     int stepCount = Mathf.RoundToInt(Angle * _resolution);
//     //     float stepAngleSize = Angle / stepCount;
//     //     List<Vector3> viewPoints = new List<Vector3>();
//     //     for (int i = 0; i <= stepCount; i++)
//     //     {
//     //         float angle = transform.eulerAngles.z - Angle / 2 + stepAngleSize * i;
//     //         ViewCastInfo newViewCast = ViewCast(angle);
//     //         viewPoints.Add(newViewCast.point);
//     //     }
//     //     int vertexCount = viewPoints.Count + 1;
//     //     Vector3[] vertices = new Vector3[vertexCount];
//     //     int[] triangles = new int[(vertexCount - 2) * 3];
//     //     vertices[0] = Vector3.zero;
//     //     for (int i = 0; i < vertexCount - 1; i++)
//     //     {
//     //         vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
//     //         if (i < vertexCount - 2)
//     //         {
//     //             triangles[i * 3] = 0;
//     //             triangles[i * 3 + 1] = i + 1;
//     //             triangles[i * 3 + 2] = i + 2;
//     //         }
//     //     }
//     //     viewMesh.Clear();
//     //     viewMesh.vertices = vertices;
//     //     viewMesh.triangles = triangles;
//     //     viewMesh.RecalculateNormals();
//     // }
//     // void LateUpdate()
//     // {
//     //     DrawFOV();
//     // }
//     void OnDrawGizmos()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawLine(transform.position, transform.position + transform.up * Radius);
//         // ! borked
//         // Gizmos.color = Color.magenta;
//         // Gizmos.DrawLine(transform.position, transform.position + new Vector3(Radius * Mathf.Cos(Angle / 2), Radius * Mathf.Sin(Angle / 2), 0f));
//         // Gizmos.DrawLine(transform.position, transform.position + new Vector3(-Radius * Mathf.Cos(-Angle / 2), -Radius * Mathf.Sin(-Angle / 2), 0f));
//         // Gizmos.DrawWireSphere(transform.position, Radius);
//     }
//     // #endregion
//     // called on parent active toggle ?
//     void OnEnable()
//     {
//         StartCoroutine("FindTargetsWithDelay", 0.2f);
//     }
//     void OnDisable()
//     {
//         StopCoroutine("FindTargetsWithDelay");
//     }
//     // public void SetActive(bool value)
//     // {
//     //     if (gameObject.activeSelf != value)
//     //         gameObject.SetActive(value);
//     // }
//     IEnumerator FindTargetsWithDelay(float delay)
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(delay);
//             FindTargets();
//         }
//     }
//     void FindTargets()
//     {
//         _targets.Clear();
//         Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, Radius, GameVariables.ScanLayerTarget);
//         for (int i = 0; i < targets.Length; i++)
//         {
//             // exclude trigger
//             if (targets[i].isTrigger)
//                 continue;
//             Transform target = targets[i].transform;
//             if (target.position == transform.position)
//                 continue;
//             // // * testing invisible
//             // if (target.gameObject.layer == game_variables.Instance.LayerPlayer && target.GetComponent<data_player>().ModeInvisible)
//             //     continue;
//             // exclude dead
//             if (target.gameObject.layer == GameVariables.LayerCreature)
//                 if (target.GetComponent<Creature>().HealthInst == 0)
//                     continue;
//             Vector3 direction = (target.position - transform.position).normalized;
//             if (Vector3.Angle(transform.up, direction) <= Angle / 2f)
//             {
//                 float distance = Vector3.Distance(transform.position, target.position);
//                 // RaycastHit2D hit;
//                 if (!Physics2D.Raycast(transform.position, direction, distance, GameVariables.ScanLayerObstruction))
//                     // player mob interact item
//                     _targets.Add(target);
//             }
//         }
//         // print(targets?.Length);
//     }
//     public Vector3 Position
//     {
//         get { return transform.position; }
//     }
//     public List<Transform> Targets
//     {
//         get { return _targets; }
//     }
//     public float Rotation
//     {
//         get { return transform.eulerAngles.z; }
//         set { transform.eulerAngles = Vector3.forward * value; }
//     }
// }