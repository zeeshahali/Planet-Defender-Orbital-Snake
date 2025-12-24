using System;
using System.Collections.Generic;
using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Fireball/ProjectileSpawnConfig", fileName = "ProjectileSpawnConfig",
        order = 0)]
    public class ProjectileSpawnConfig : ScriptableObject
    {
        public float SpawnDelay = 1;
        public float SpawnRadiusMultiplier = 1;
        
        public List<ProjectileSpawnData> ProjectileSpawnData;
    }

    [Serializable]
    public struct ProjectileSpawnData
    {
        public ProjectileType ProjectileType;
        public float SpawnProbability;
        public Projectile Projectile;
    }
}