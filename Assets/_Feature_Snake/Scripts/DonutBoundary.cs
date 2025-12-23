using UnityEngine;

public class DonutBoundary : MonoBehaviour
{
    public Vector2 centerPoint = Vector2.zero;
    public float innerRadius = 3f;
    public float outerRadius = 10f;

    public void ApplyConstraint(Transform target, Rigidbody2D rb = null)
    {
        Vector2 currentPos = target.position;
        float distanceFromCenter = Vector2.Distance(currentPos, centerPoint);

        if (distanceFromCenter > outerRadius || distanceFromCenter < innerRadius)
        {
            Vector2 directionFromCenter = (distanceFromCenter > 0.01f) 
                ? (currentPos - centerPoint).normalized 
                : (Vector2)target.up;

            float clampedDistance = Mathf.Clamp(distanceFromCenter, innerRadius, outerRadius);
            Vector2 newPos = centerPoint + (directionFromCenter * clampedDistance);

            if (rb != null) rb.position = newPos;
            target.position = newPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerPoint, outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerPoint, innerRadius);
    }
}