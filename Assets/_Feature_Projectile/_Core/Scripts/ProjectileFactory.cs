using System.Collections.Generic;
using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    public class ProjectileFactory
    {
        private readonly Dictionary<ProjectileType, Projectile> _projectiles = new Dictionary<ProjectileType, Projectile>();

        public ProjectileFactory(List<ProjectileSpawnData> projectileSpawnData)
        {
            foreach (var data in projectileSpawnData)
            {
                _projectiles.Add(data.ProjectileType,  data.Projectile);
            }
        }

        public Fireball CreateFireball(Vector3 position, Transform target, ILoopDetection loopDetection)
        {
            Fireball fireball = Object.Instantiate(_projectiles[ProjectileType.Fireball], position, Quaternion.identity) as Fireball;
            InitializeProjectile(fireball, target, loopDetection);

            return fireball;
        }
        
        public SpikeBall CreateSpikeBall(Vector3 position, Transform target, ILoopDetection loopDetection, 
            IBodyManipulation bodyManipulation)
        {
            SpikeBall spikeBall = Object.Instantiate(_projectiles[ProjectileType.SpikeBall], position, Quaternion.identity) as SpikeBall;
            InitializeProjectile(spikeBall, target, loopDetection);
            spikeBall.UpdateBodyManipulation(bodyManipulation);
            
            return spikeBall;
        }

        public Snowball CreateSnowball(Vector3 position, Transform target, ILoopDetection loopDetection)
        {
            Snowball snowball = Object.Instantiate(_projectiles[ProjectileType.Snowball], position, Quaternion.identity) as Snowball;
            InitializeProjectile(snowball, target, loopDetection);
            
            return snowball;
        }

        private void InitializeProjectile(Projectile projectile, Transform target, ILoopDetection loopDetection)
        {
            projectile.SetTarget(target);
            projectile.SetLoopDetection(loopDetection);
        }
    }
}