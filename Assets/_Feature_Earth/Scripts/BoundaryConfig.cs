using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Configs/BoundaryConfigs", fileName = "BoundaryConfig", order = 0)]
public class BoundaryConfig : ScriptableObject
{
    [SerializeField] private EarthConfig EarthConfig;
    public float OuterRadiusMultplier = 2;
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
}