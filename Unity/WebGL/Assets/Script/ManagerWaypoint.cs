using UnityEngine;
using System.Collections.Generic;
// ? test static over singleton
// ? manually setting position .5 .5 5 to match grid and mob layer
public class ManagerWaypoint : MonoBehaviour
{
    public static ManagerWaypoint Instance;
    private List<Waypoint> _waypoints;
    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
        // 
        _waypoints = new List<Waypoint>();
        foreach (Transform child in transform) _waypoints.Add(child.GetComponent<Waypoint>());
    }
    // ? slower with more waypoints, two tier hierarchy, first filtered via id, second via position
    public Waypoint GetWaypointNearest(Vector3 position)
    {
        float distance = float.MaxValue;
        Waypoint temp = null;
        foreach (Waypoint waypoint in _waypoints)
        {
            if (Vector3.Distance(position, waypoint.Position) < distance)
            {
                distance = Vector3.Distance(position, waypoint.Position);
                temp = waypoint;
            }
        }
        return temp;
    }
    // * testing null error on reload
    void OnDestroy()
    {
        Instance = null;
    }
}