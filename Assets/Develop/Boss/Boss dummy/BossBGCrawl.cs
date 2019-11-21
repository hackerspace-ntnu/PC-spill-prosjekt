/* This is a state / behaviour for the boss. While active, the boss will calculate a path on the wall to 
 * crawl, and follow the path bezier-style. At the end of the path, or if otherwise this component
 * is disabled, the crawl will end. A new path will be calculated and followed when the component is enabled.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBGCrawl : MonoBehaviour
{
    // Setting some public variables for potential difficulty adjustments
    // NB! DO NOT SET NUMBER OF CHECKPOINTS TO ABOVE 20
    public float pathLength = 5;
    public float timeSteps = 100;
    public int numberOfCheckpoints = 5;

    // Private variables for execution of logic and movement
    private float side = 1;
    private List<Vector2> positions;
    private List<Quaternion> rotations;
    private int count;
    private Vector2 startDirection;
    private List<List<Vector2>> mission;


    // Update runs after the awake-initialization. A counter counts us through the path and updates positions and rotations. 
    // Lists are used to not overload the update with too many tasks at the same time. Might cause lag on the frame the state
    // awakens, in that case we can do a single calculation every update instead. Disables the behaviour when all positions are read.
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


    // Makes the path of key-positions, and calculates the bezier curve points. The key-positions must be pathLength apart and be
    // within the boundaries of the arena. The first path calculated is a quadratic bezier curve for easier calculatinos at the start.
    // Thereafter the rest of the curves are cubic curves. The total number of curves is decided by the numberOfCheckpoints variable.
    // The method returns the a list of lists with important path points for calculation of the bezier curve. The first index indicates 
    // the path number, and the second index indicates the points. The first path has indexes 0: start, 1: end, and 2: quadratic point. 
    // The remaining paths have indexes 0: start, 1: end, 2: first cubic point, and 3: second cubic point.
    private List<List<Vector2>> FindPaths()
    {
        Vector2 startPos = transform.position;
        List<List<Vector2>> paths = new List<List<Vector2>>();

        // Make the first (quadratic) bezier path and update startPos and startDirection for the next curve.
        paths.Add(new List<Vector2>());
        paths[0].Add(startPos);
        paths[0].Add(FindEndPoint(startPos));
        paths[0].Add(FindQuadraticPoint(paths[0][0], paths[0][1]));
        startPos = paths[0][1];
        startDirection = ((paths[0][1] - paths[0][2]) * 2).normalized * (pathLength / 2);

        // Make cubic bezier paths and update startPos and startDirection for the next curve. The first cubic point must be straight forward
        // from the starting direction to make sure the whole movement is smooth and and without sharp turns.
        for (int i = 1; i < numberOfCheckpoints; i++)
        {
            paths.Add(new List<Vector2>());
            paths[i].Add(startPos);
            paths[i].Add(FindEndPoint(startPos));
            paths[i].Add(startPos + startDirection);
            paths[i].Add(FindCubicPoint2(paths[i][0], paths[i][1], startDirection));
            startDirection = GetFirstDerivativeCubic(paths[i][0], paths[i][2], paths[i][3], paths[i][1], 1).normalized * (pathLength / 2);
            startPos = paths[i][1];
        }

        return paths;
    }


    // Find the endpoint of a bezier path. The point must meet the criteria of being a set distance pathLength away from the starting 
    // position, and be within the boundaries of the arena. Returns the point.
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


    // Find the quadratic point by rotating the end-point 45 degrees in a direction with the start position as the pivot. Returns the point.
    private Vector2 FindQuadraticPoint(Vector2 start, Vector2 end)
    {
        Vector2 point = end - start;
        point = (Vector2)(Quaternion.Euler(0, 0, 45 * side) * point) + start;
        return point;
    }


    // Find the second cubic point by rotating the end-point 45 degrees in a direction with the start position as the pivot. 
    // The direction of rotation depends on the angle between the starting point and this point. If the angle is relatively close to 180
    // degrees, then the second cubic point must rotate the other way. This is to avoid a very sharp 180 degrees turn in the bezier curve.
    // Returns the point.
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

    /// <summary>
    /// Check if the point is within the bounds of the boss arena and. Returns the answer.
    /// </summary>
    /// <param name="point">Point in 2D space</param>
    /// <returns>Returns true if the point is within the arena</returns>
    private bool IsValidDestination(Vector2 point)
    {
        // Values must be correct when implemented in the game
        if (point.x > -7 && point.x < 7 && point.y > -4 && point.y < 4) return true;
        else return false;
    }

    /// <summary>
    /// Calculates all the bezier curve positions from the mission bezier path. The first index of the mission path list is quadratic, the rest
    /// are cubic. The number of points on the curve is equal to timeSteps. Returns a list of all points for the whole mission.
    /// </summary>
    /// <returns>Returns a list of all positions on bezier curves</returns>
    private List<Vector2> FindBezierPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        List<List<Vector2>> missionCopy = new List<List<Vector2>>(mission);
        float step = 1 / timeSteps;

        // Quadratic part.
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

        // Cubic part.
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

    /// <summary>
    /// Calculates all the bezier curve rotations from the mission bezier path. The first index of the mission path list is quadratic, the rest
    /// are cubic. The number of points on the curve is equal to timeSteps. Returns a list of all rotations for the whole mission.
    /// </summary>
    /// <returns>Returns a list of all rotations on bezier curves</returns>
    private List<Quaternion> FindBezierRotations()
    {
        List<Quaternion> rotations = new List<Quaternion>();
        List<List<Vector2>> missionCopy = new List<List<Vector2>>(mission);
        float step = 1 / timeSteps;
        float angle;

        // Quadratic part.
        float t = step;
        while (t < 1 + step / 2)
        {
            Vector2 direction = GetFirstDerivativeQuadratic(missionCopy[0][0], missionCopy[0][2], missionCopy[0][1], t);
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            rotations.Add(Quaternion.AngleAxis(angle, Vector3.forward));
            t += step;
        }
        missionCopy.RemoveAt(0);

        // Cubic part.
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

    /// <summary>
    /// Gets the direction of movement at a specific point on the quadratic bezier curve.
    /// </summary>
    /// <param name="p0">Starting point</param>
    /// <param name="p1">Curve point</param>
    /// <param name="p2">End point</param>
    /// <param name="t">Point in time on the bezier curve between 0 (start) and 1 (end)</param>
    /// <returns>Returns the velocity at time t</returns>
    public Vector2 GetFirstDerivativeQuadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        return
             2f * (1f - t) * (p1 - p0) +
             2f * t * (p2 - p1);
    }


    /// <summary>
    /// Gets the direction of movement at a specific point on the cubic bezier curve. 
    /// </summary>
    /// <param name="p0">Starting point</param>
    /// <param name="p1">First curve point</param>
    /// <param name="p2">Second curve point</param>
    /// <param name="p3">End point</param>
    /// <param name="t">Point in time on the bezier curve between 0 (start) and 1 (end)</param>
    /// <returns>Returns the velocity at time t</returns>
    public Vector2 GetFirstDerivativeCubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }


    /// <summary>
    /// Finds a new mission, positions and rotations. Resets count.
    /// </summary>
    private void OnEnable()
    {
        mission = FindPaths();
        positions = FindBezierPositions();
        rotations = FindBezierRotations();
        count = 0;
    }


    /// <summary>
    /// Sends a message to the state machine that this state has ended
    /// </summary>
    private void OnDisable()
    {
        
    }
}
