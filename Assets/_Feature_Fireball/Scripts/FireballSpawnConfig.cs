using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fireball/FireballSpawnConfig", fileName = "FireballSpawnConfig", order = 0)]
public class FireballSpawnConfig : ScriptableObject
{
    public float SpawnDelay;
    public Fireball FireballPrefab;
}