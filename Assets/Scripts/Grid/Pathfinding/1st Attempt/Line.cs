using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    const float verticalLineGradient = 1e5f;

    float gradient;
   // float yIntercept;
    Vector2 pointOnLine_1;
    Vector2 pointOnLine_2;

    float gradientPerpendicular;

    bool onApproachSide;


    // might be some bad math here
    public Line(Vector2 pointOnBothLines, Vector2 pointOnPerpendicularToTurnBoundary)
    {
        float dx = pointOnBothLines.x - pointOnPerpendicularToTurnBoundary.x;
        float dy = pointOnBothLines.y - pointOnPerpendicularToTurnBoundary.y;

        if (dx == 0)
            gradientPerpendicular = verticalLineGradient;
        else
            gradientPerpendicular = dy / dx;

        if (gradientPerpendicular == 0)
            gradient = verticalLineGradient;
        else
            gradient = -1 / gradientPerpendicular;

       // yIntercept = pointOnLine.y - gradient * pointOnLine.x;
        pointOnLine_1 = pointOnBothLines;
        pointOnLine_2 = pointOnBothLines + new Vector2(1, gradient);

        onApproachSide = false; // need this line because every field must be assigned before calling method (comment it out, you'll see)
        onApproachSide = IsPointOnApproachSide(pointOnPerpendicularToTurnBoundary);
    }

    bool IsPointOnApproachSide(Vector2 p)
    {
        return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) < 
            (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool HasCrossedLine(Vector2 p)
    {
        return IsPointOnApproachSide(p) != onApproachSide;
    }

    public void DrawWithGizmos(float length)
    {
        Vector2 lineDirection = new Vector2(1, gradient).normalized;
        Vector2 lineCenter = new Vector2(pointOnLine_1.x, pointOnLine_1.y);
        Gizmos.DrawLine(lineCenter - lineDirection * length / 2f, lineCenter + lineDirection * length / 2f);
    }
}