using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Fireball/ProjectileSpawnConfig", fileName = "ProjectileSpawnConfig",
        order = 0)]
    public class ProjectileSpawnConfig : ScriptableObject
    {
        public float SpawnDelay = 1;
        public float SpawnRadiusMultiplier = 1;
        public Fireball FireballPrefab;
        public SpikeBall SpikeBallPrefab;
        public Snowball SnowballPrefab;
    }
}