using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    public class ProjectileFactory
    {
        private Fireball _fireballPrefab;
        private SpikeBall _spikeBallPrefab;
        private Snowball _snowballPrefab;

        public ProjectileFactory(Fireball fireballPrefab, SpikeBall spikeBallPrefab, Snowball snowballPrefab)
        {
            _fireballPrefab = fireballPrefab;
            _spikeBallPrefab = spikeBallPrefab;
            _snowballPrefab = snowballPrefab;
        }

        public Fireball CreateFireball(Vector3 position, Transform target, IMovementStrategy movement,
            ILoopDetection loopDetection)
        {
            Fireball fireball = Object.Instantiate(_fireballPrefab, position, Quaternion.identity);
            InitializeProjectile(fireball, target, movement, loopDetection);

            return fireball;
        }
        
        public SpikeBall CreateSpikeBall(Vector3 position, Transform target, IMovementStrategy movement,
            ILoopDetection loopDetection, IBodyManipulation bodyManipulation)
        {
            SpikeBall spikeBall = Object.Instantiate(_spikeBallPrefab, position, Quaternion.identity);
            InitializeProjectile(spikeBall, target, movement, loopDetection);
            spikeBall.UpdateBodyManipulation(bodyManipulation);
            
            return spikeBall;
        }

        public Snowball CreateSnowball(Vector3 position, Transform target, IMovementStrategy movement,
            ILoopDetection loopDetection)
        {
            Snowball snowball = Object.Instantiate(_snowballPrefab, position, Quaternion.identity);
            InitializeProjectile(snowball, target, movement, loopDetection);
            
            return snowball;
        }

        private void InitializeProjectile(Projectile projectile, Transform target, IMovementStrategy movement, ILoopDetection loopDetection)
        {
            projectile.SetTarget(target);
            projectile.SetMovementStrategy(movement);
            projectile.SetLoopDetection(loopDetection);
        }
    }
}