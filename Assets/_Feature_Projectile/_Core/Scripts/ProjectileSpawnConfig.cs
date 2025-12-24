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

        public ProjectileSpawnData GetProjectileSpawnData(ProjectileType type)
        {
            var data =  ProjectileSpawnData[0];
            foreach (var spawnData in ProjectileSpawnData)
            {
                if(spawnData.ProjectileType != type) continue;
                data = spawnData;
                break;
            }

            return data;
        }
    }

    [Serializable]
    public struct ProjectileSpawnData
    {
        public ProjectileType ProjectileType;
        public float SpawnProbability;
        public Projectile Projectile;

        public void UpdateSpawnProbability(float probability)
        {
            SpawnProbability = probability;
        }
    }
}