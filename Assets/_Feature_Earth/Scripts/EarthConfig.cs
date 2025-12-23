using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Earth/EarthConfig", fileName = "EarthConfig", order = 0)]
public class EarthConfig : ScriptableObject
{
    public float StartingHealth;
    public float DamagePerHit;
    
    public float GravityIntensity;
    public float Radius;
}