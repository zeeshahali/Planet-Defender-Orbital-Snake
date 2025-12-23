using System.Collections.Generic;
using UnityEngine;

public class SnakeLoopDetector : MonoBehaviour
{
    [SerializeField] private float closureThreshold = 1.0f;
    [SerializeField] private int minSegmentsForLoop = 3;

    public bool IsPointInLoop(Vector2 point, Vector2 headPos, List<GameObject> bodyParts)
    {
        int closureIndex = FindLoopClosureIndex(headPos, bodyParts);
        if (closureIndex == -1) return false;

        List<Vector2> polygon = new List<Vector2> { headPos };
        for (int i = 0; i <= closureIndex; i++)
        {
            polygon.Add(bodyParts[i].transform.position);
        }

        return IsPointInPolygon(point, polygon);
    }

    private int FindLoopClosureIndex(Vector2 headPos, List<GameObject> bodyParts)
    {
        for (int i = minSegmentsForLoop; i < bodyParts.Count; i++)
        {
            if (Vector2.Distance(headPos, bodyParts[i].transform.position) < closureThreshold)
                return i;
        }
        return -1;
    }

    private bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        bool isInside = false;
        for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
        {
            if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
            {
                isInside = !isInside;
            }
        }
        return isInside;
    }
}