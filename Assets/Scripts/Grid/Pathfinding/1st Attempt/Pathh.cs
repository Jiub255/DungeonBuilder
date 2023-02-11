using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathh
{
    public readonly Vector2[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;

    public Pathh(Vector2[] waypoints, Vector3 startPosition, float turnDistance)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 previousPoint = startPosition;
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = lookPoints[i];
            Vector2 directionToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - directionToCurrentPoint * turnDistance;
         
            // something wrong here. the last line is out of place, maybe out of order?
            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - directionToCurrentPoint * turnDistance);
            previousPoint = turnBoundaryPoint;
        }
    }

    public void DrawWithGizmos()
    {
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Gizmos.color = Random.ColorHSV(.5f,1f,.5f,1f,.5f,1f,1f,1f);

            Gizmos.DrawCube(lookPoints[i], Vector2.one);

            turnBoundaries[i].DrawWithGizmos(10);
        }

/*        Gizmos.color = Color.black;
        foreach (Vector2 p in lookPoints)
        {
            Gizmos.DrawCube(p, Vector2.one);
        }

        Gizmos.color = Color.white;
        foreach (Line l in turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }*/
    }
}