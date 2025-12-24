using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Configs/BoundaryConfigs", fileName = "BoundaryConfig", order = 0)]
public class BoundaryConfig : ScriptableObject
{
    [SerializeField] private EarthConfig EarthConfig;

    public float InnerRadiusMultplier = 1;
    public Vector2 CenterPoint = Vector2.zero;

    public float padding = 0.5f;

    public float GetInnerRadius()
    {
        return EarthConfig.Radius * InnerRadiusMultplier;
    }

    // For rectangular boundary, we'll use these instead of radius
    public float GetLeftBound(Camera cam) => GetScreenBounds(cam).x;
    public float GetRightBound(Camera cam) => GetScreenBounds(cam).y;
    public float GetBottomBound(Camera cam) => GetScreenBounds(cam).z;
    public float GetTopBound(Camera cam) => GetScreenBounds(cam).w;
        
    private Vector4 GetScreenBounds(Camera cam)
    {
        if (!cam.orthographic) 
            return Vector4.zero;
                
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
            
        // Calculate boundaries with padding
        float left = -halfWidth + padding;
        float right = halfWidth - padding;
        float bottom = -halfHeight + padding;
        float top = halfHeight - padding;
            
        return new Vector4(left, right, bottom, top);
    }
    
    public Vector2 GetRandomConstrainedPosition(Camera cam, float radiusMultiplier)
    {
        var centerPoint = CenterPoint;
        float innerRadius = GetInnerRadius();

        // 1. Get the rectangular boundaries
        float left = GetLeftBound(cam) + centerPoint.x;
        float right = GetRightBound(cam) + centerPoint.x;
        float bottom = GetBottomBound(cam) + centerPoint.y;
        float top = GetTopBound(cam) + centerPoint.y;

        Vector2 randomPos;
        int attempts = 0;

        // 2. Keep picking a point until it is outside the inner circle
        do
        {
            float x = Random.Range(left, right);
            float y = Random.Range(bottom, top);
            randomPos = new Vector2(x, y);
        
            attempts++;
            // Safety check to prevent infinite loops if the circle is larger than the rectangle
            if (attempts > 100) break; 

        } while (Vector2.Distance(randomPos, centerPoint) < innerRadius * radiusMultiplier);

        return randomPos;
    }
}