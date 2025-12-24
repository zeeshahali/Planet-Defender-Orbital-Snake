using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    public class Fireball : Projectile
    {

        protected override void Awake()
        {
            base.Awake();
            ProjectileType = ProjectileType.Fireball;
        }

        public override void HandleCollision(Collision other)
        {
            base.HandleCollision(other);
            if (other.gameObject.CompareTag("Player"))
            {
                if (_loopDetection.IsPointInLoop(transform.position))
                {
                    EventManager.TriggerFireballDestroyed();
                    DestroyProjectile();
                }
            }
        }
    }
}
