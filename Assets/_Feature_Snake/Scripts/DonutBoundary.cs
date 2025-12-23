using UnityEngine;

public class DonutBoundary : MonoBehaviour
{
    [SerializeField] private BoundaryConfig BoundaryConfig;

    private Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void ApplyConstraint(Transform target, Rigidbody2D rb = null)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        Vector2 currentPos = target.position;
        var centerPoint = BoundaryConfig.CenterPoint;
        
        // Get screen boundaries
        float leftBound = BoundaryConfig.GetLeftBound(mainCamera) + centerPoint.x;
        float rightBound = BoundaryConfig.GetRightBound(mainCamera) + centerPoint.x;
        float bottomBound = BoundaryConfig.GetBottomBound(mainCamera) + centerPoint.y;
        float topBound = BoundaryConfig.GetTopBound(mainCamera) + centerPoint.y;
        
        // Apply inner circular constraint (if you still want this)
        Vector2 center = centerPoint;
        float distanceFromCenter = Vector2.Distance(currentPos, center);
        
        if (distanceFromCenter < BoundaryConfig.GetInnerRadius())
        {
            Vector2 directionFromCenter = (distanceFromCenter > 0.01f) 
                ? (currentPos - center).normalized 
                : target.up;
            currentPos = center + directionFromCenter * BoundaryConfig.GetInnerRadius();
        }
        
        // Apply rectangular screen constraint
        currentPos.x = Mathf.Clamp(currentPos.x, leftBound, rightBound);
        currentPos.y = Mathf.Clamp(currentPos.y, bottomBound, topBound);
        
        // Apply position
        if (rb != null) 
            rb.position = currentPos;
        else 
            target.position = currentPos;
    }


    private void OnDrawGizmos()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;
        
        var centerPoint = BoundaryConfig.CenterPoint;
        
        float left = BoundaryConfig.GetLeftBound(mainCamera) + centerPoint.x;
        float right = BoundaryConfig.GetRightBound(mainCamera) + centerPoint.x;
        float bottom = BoundaryConfig.GetBottomBound(mainCamera) + centerPoint.y;
        float top = BoundaryConfig.GetTopBound(mainCamera) + centerPoint.y;
        
        // Draw outer rectangle (screen bounds)
        Gizmos.color = Color.green;
        Vector3 topLeft = new Vector3(left, top, 0);
        Vector3 topRight = new Vector3(right, top, 0);
        Vector3 bottomLeft = new Vector3(left, bottom, 0);
        Vector3 bottomRight = new Vector3(right, bottom, 0);
        
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
        
        // Draw inner circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(BoundaryConfig.CenterPoint, BoundaryConfig.GetInnerRadius());
    }
}