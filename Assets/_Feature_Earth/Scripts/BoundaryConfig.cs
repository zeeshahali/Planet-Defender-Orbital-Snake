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
        return EarthConfig.Radius * OuterRadiusMultplier;
    }
}