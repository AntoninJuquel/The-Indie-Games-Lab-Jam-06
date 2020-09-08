using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathManager : MonoBehaviour
{
    [SerializeField] List<Vector3> waypoints;
    [Header("Color")]
    [SerializeField] Color playColor;
    [SerializeField] Color pauseColor;
    [SerializeField] Color stopColor;
    [SerializeField] Train train;
    PathCreator path;
    LineRenderer lr;
    public static PathManager Instance;
    bool started;

    private void Awake()
    {
        Instance = this;
        lr = GetComponent<LineRenderer>();
        path = GetComponent<PathCreator>();
        lr.positionCount = waypoints.Count;
    }

    public void AddWaypoint(Vector3 position)
    {
        waypoints.Add(position);
        lr.positionCount = waypoints.Count;
        Vector3[] linepath = new Vector3[waypoints.Count];
        for (int i = 0; i < waypoints.Count; i++)
        {
            linepath.SetValue(waypoints[i], i);
        }
        lr.SetPositions(linepath);
    }
    public Vector3 GetLastWaypointPosition(Vector3 position)
    {
        if (waypoints.Count > 0)
            return waypoints[waypoints.Count - 1];
        return position;
    }

    public bool StartPath()
    {
        bool canStart = waypoints.Count > 0;
        if (canStart)
        {


            lr.startColor = lr.endColor = playColor;
            if (!started)
            {
                GenerateVertexPath();
                train.StartTrain();
                started = true;
            }
            else
            {
                train.StartTrain();
            }
        }
        return canStart;
    }
    public void PausePath()
    {
        lr.startColor = lr.endColor = pauseColor;
        train.PauseTrain();
    }
    public void ResetPath()
    {
        started = false;
        waypoints = new List<Vector3>();
        lr.startColor = lr.endColor = stopColor;
        lr.positionCount = waypoints.Count;
        train.ResetTrain();
    }

    void GenerateVertexPath()
    {
        BezierPath bezierPath = new BezierPath(waypoints, false, PathSpace.xyz);
        
        path.bezierPath = bezierPath;
        path.bezierPath.GlobalNormalsAngle = 90f;
    }
}
