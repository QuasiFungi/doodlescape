using UnityEngine;
using System.Collections.Generic;
public class Waypoint : MonoBehaviour
{
    [SerializeField] protected float _radius = .5f;
    [Tooltip("Directional links")] [SerializeField] protected List<Waypoint> _neighbours = new List<Waypoint>();
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 1f, 1f);
        foreach (Waypoint next in _neighbours)
            if (next)
                Gizmos.DrawLine(transform.position, next.Position);
        Gizmos.color = new Color(1f, 0f, 1f, .5f);
        Gizmos.DrawSphere(transform.position, _radius);
    }
    // position + random deviation
    public Vector3 PositionRandom()
    {
        Vector3 temp = transform.up * Random.Range(-1f, 1f) + transform.right * Random.Range(-1f, 1f);
        temp = temp.magnitude > 1f ? temp.normalized * _radius : temp * _radius;
        return transform.position + temp;
    }
    public Waypoint GetNext()
    {
        if (_neighbours.Count == 0)
            return null;
        return _neighbours[Random.Range(0, _neighbours.Count)];
    }
    public bool IsWaypoint(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) < _radius;
    }
    public Vector3 Position
    {
        get { return transform.position; }
    }
}