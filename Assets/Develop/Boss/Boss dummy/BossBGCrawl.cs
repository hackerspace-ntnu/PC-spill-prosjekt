using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBGCrawl : MonoBehaviour
{
    public float pathLength = 5;
    public float timeSteps = 100;
    public int numberOfCheckpoints = 5;

    private float side = 1;
    private List<Vector2> positions;
    private List<Quaternion> rotations;
    private int count;
    private Vector2 startDirection;
    private List<List<Vector2>> mission;


    private void Update()
    {
        transform.position = positions[count];
        transform.rotation = rotations[count];
        count++;
        if (count == positions.Count)
        {
            GetComponent<BossBGCrawl>().enabled = false;
        }
    }


    // 
    private List<List<Vector2>> FindPaths()
    {
        Vector2 startPos = transform.position;
        List<List<Vector2>> paths = new List<List<Vector2>>();

        // Make the first (quadratic) bezier curve
        paths.Add(new List<Vector2>());
        paths[0].Add(startPos);
        paths[0].Add(FindEndPoint(startPos));
        paths[0].Add(FindQuadraticPoint(paths[0][0], paths[0][1]));
        startPos = paths[0][1];
        startDirection = ((paths[0][1] - paths[0][2]) * 2).normalized * (pathLength / 2);

        // Make cubic bezier curves
        for (int i = 1; i < numberOfCheckpoints; i++)
        {
            paths.Add(new List<Vector2>());
            paths[i].Add(startPos);
            paths[i].Add(FindEndPoint(startPos));
            paths[i].Add(startPos + startDirection);
            paths[i].Add(FindCubicPoint2(paths[i][0], paths[i][1], startDirection));
            startDirection = GetFirstDerivativeCubic(paths[i][0], paths[i][2], paths[i][3], paths[i][1], 1).normalized * (pathLength / 2);
            startPos = paths[i][1];
            
         
            
            /*
            GameObject Start = Instantiate(new GameObject(), paths[i][0], Quaternion.identity);
            Start.name = "Start
            GameObject End = Instantiate(new GameObject(), paths[i][1], Quaternion.identity);
            End.name = "End";
            GameObject Quad = Instantiate(new GameObject(), paths[i][2], Quaternion.identity);
            Quad.name = "Quad";
            */
        }

        return paths;
    }


    // 
    private Vector2 FindEndPoint(Vector2 start)
    {
        Vector2 point = new Vector2();

        bool check = false;
        while (!check)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            float x = Mathf.Cos(angle) * pathLength;
            float y = Mathf.Sin(angle) * pathLength;
            point = new Vector2(x, y) + start;

            check = IsValidDestination(point);
        }
        
        return point;
    }


    // 
    private Vector2 FindQuadraticPoint(Vector2 start, Vector2 end)
    {
        Vector2 point = end - start;
        point = (Vector2)(Quaternion.Euler(0, 0, 45 * side) * point) + start;
        return point;
    }


    //
    private Vector2 FindCubicPoint2(Vector2 start, Vector2 end, Vector2 direction)
    {
        Vector2 point = end - start;
        point = (Vector2)(Quaternion.Euler(0, 0, 45 * side) * point) + start;

        float startAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float startToPointAngle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;

        float angleDiff = Mathf.Abs(startToPointAngle - startAngle);
        if (angleDiff > 180)
        {
            angleDiff = 360 - angleDiff;
        }

        if (angleDiff > 135)
        {
            side *= -1;
            point = end - start;
            point = (Vector2)(Quaternion.Euler(0, 0, 45 * side) * point) + start;
        }

        return point;
    }


    // Check if the point is within the bounds of the boss arena
    private bool IsValidDestination(Vector2 point)
    {
        // Values should be dynamic when implemented.
        if (point.x > -7 && point.x < 7 && point.y > -4 && point.y < 4) return true;
        else return false;
    }


    //
    private List<Vector2> FindBezierPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        List<List<Vector2>> missionCopy = new List<List<Vector2>>(mission);
        float step = 1 / timeSteps;

        // Quadratic part
        float t = step;
        while (t < 1 + step / 2)
        {
            Vector2 pos = 
                Mathf.Pow((1 - t), 2) * missionCopy[0][0] + 
                2 * (1 - t) * t * missionCopy[0][2] + 
                Mathf.Pow(t, 2) * missionCopy[0][1];
            positions.Add(pos);
            t += step;
        }
        missionCopy.RemoveAt(0);

        // Cubic part
        foreach (var path in missionCopy)
        {
            Vector2 p0 = path[0];
            Vector2 p3 = path[1];
            Vector2 p1 = path[2];
            Vector2 p2 = path[3];
            t = step;
            while (t < 1 + step / 2)
            {
                Vector2 pos = 
                    Mathf.Pow((1 - t), 3) * p0 + 
                    3 * Mathf.Pow((1 - t), 2) * t * p1 + 
                    3 * (1 - t) * Mathf.Pow(t,2) * p2 + 
                    Mathf.Pow(t,3) * p3;
                positions.Add(pos);
                t += step;
            }
        }

        return positions;
    }


    // 
    private List<Quaternion> FindBezierRotations()
    {
        List<Quaternion> rotations = new List<Quaternion>();
        List<List<Vector2>> missionCopy = new List<List<Vector2>>(mission);
        float step = 1 / timeSteps;
        float angle;

        // Quadratic part
        float t = step;
        while (t < 1 + step / 2)
        {
            Vector2 direction = GetFirstDerivativeQuadratic(missionCopy[0][0], missionCopy[0][2], missionCopy[0][1], t);
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            rotations.Add(Quaternion.AngleAxis(angle, Vector3.forward));
            t += step;
        }
        missionCopy.RemoveAt(0);

        // Cubic part
        foreach (var path in missionCopy)
        {
            Vector2 p0 = path[0];
            Vector2 p3 = path[1];
            Vector2 p1 = path[2];
            Vector2 p2 = path[3];
            t = step;
            while (t < 1 + step / 2)
            {
                Vector2 direction = GetFirstDerivativeCubic(p0, p1, p2, p3, t);
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                rotations.Add(Quaternion.AngleAxis(angle, Vector3.forward));
                t += step;
            }
        }

        return rotations;
    }


    // Finds the rotation direction vector or the quadratic bezier curve
    public Vector2 GetFirstDerivativeQuadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        return
             2f * (1f - t) * (p1 - p0) +
             2f * t * (p2 - p1);
    }


    // Finds the rotation direction vector for the cubic bezier curve
    public Vector2 GetFirstDerivativeCubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }


    // Starts the walking coroutine. This indicates that the state machine has activated this state
    private void OnEnable()
    {
        mission = FindPaths();
        positions = FindBezierPositions();
        rotations = FindBezierRotations();
        count = 0;
    }


    // End any current movement. This is because the statemachine changes
    private void OnDisable()
    {
        
    }
}
