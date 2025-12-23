using UnityEngine;

public class DonutBoundary : MonoBehaviour
{
    [SerializeField] private BoundaryConfig BoundaryConfig;

    public void ApplyConstraint(Transform target, Rigidbody2D rb = null)
    {
        var centerPoint = BoundaryConfig.CenterPoint;
        var innerRadius = BoundaryConfig.GetInnerRadius();
        var outerRadius = BoundaryConfig.GetOuterRadius();
        
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
        Gizmos.DrawWireSphere(BoundaryConfig.CenterPoint, BoundaryConfig.GetOuterRadius());
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(BoundaryConfig.CenterPoint, BoundaryConfig.GetInnerRadius());
    }
}