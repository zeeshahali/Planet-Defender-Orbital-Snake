using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    public class Snowball : Projectile
    {
        protected override void Awake()
        {
            base.Awake();
            ProjectileType = ProjectileType.Snowball;
        }

        public override void HandleCollision(Collision other)
        {
            base.HandleCollision(other);
            if (other.gameObject.CompareTag("Player"))
            {
                if (_loopDetection.IsPointInLoop(transform.position))
                {
                    DestroyProjectile();
                }
            }
        }
    }
}