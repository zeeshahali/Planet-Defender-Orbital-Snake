using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Earth/EarthConfig", fileName = "EarthConfig", order = 0)]
public class EarthConfig : ScriptableObject
{
    public float StartingHealth;
    
    [Range(1f, 10f)]
    public float Radius = 10f;
}