using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Configs/BoundaryConfigs", fileName = "BoundaryConfig", order = 0)]
public class BoundaryConfig : ScriptableObject
{
    [SerializeField] private EarthConfig EarthConfig;
    public float OuterRadiusMultplier = 2;
    public float InnerRadiusMultplier = 1;
    public Vector2 CenterPoint = Vector2.zero;

    public float GetInnerRadius()
    {
        return EarthConfig.Radius * InnerRadiusMultplier;
    }

    public float GetOuterRadius()
    {
        float screenRadius = GetCornerRadius(Camera.main);
        return screenRadius;
    }
    
    public float GetCornerRadius(Camera cam)
    {
        if (!cam.orthographic) return 0f;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // Calculate distance from center to a corner (0,0 to halfWidth, halfHeight)
        return Mathf.Min(halfWidth, halfHeight) - 0.5f;
    }
}