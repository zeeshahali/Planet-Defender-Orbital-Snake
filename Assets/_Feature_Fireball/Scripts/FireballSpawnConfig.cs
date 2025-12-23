using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fireball/FireballSpawnConfig", fileName = "FireballSpawnConfig", order = 0)]
public class FireballSpawnConfig : ScriptableObject
{
    public float SpawnDelay = 1;
    public float SpawnRadiusMultiplier = 1;
    public Fireball FireballPrefab;
}