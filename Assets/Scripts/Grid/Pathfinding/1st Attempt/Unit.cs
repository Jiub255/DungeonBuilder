using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;

    public Transform target;
    public float speed = 5f;
    public float turnSpeed = 3f;
    public float turnDistance = 5f;

    Pathh path;

    bool followingPath = true;
    int pathIndex = 0;

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    public void OnPathFound(Vector2[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Pathh(waypoints, transform.position, turnDistance);

            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        followingPath = true; 
        pathIndex = 0;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, 
            (Vector3)path.lookPoints[pathIndex] - transform.position);

        while (followingPath)
        {
            Vector2 position2d = new Vector2(transform.position.x, transform.position.y);

            while (path.turnBoundaries[pathIndex].HasCrossedLine(position2d)) // will 'if' work instead of while
            {
                if (pathIndex == path.finishLineIndex) 
                {
                    print("reached end of path");
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                    print("path index: " + pathIndex);
                }
            }

            if (followingPath) // dont need this if, unless you change the above while back to an if? 
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, 
                    (Vector3)path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.up * Time.deltaTime * speed);
               // print(transform.position);
            }

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}