using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    public class Fireball : Projectile
    {
        [SerializeField] private GameObject _explosionParticle;

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

        public override void DestroyProjectile()
        {
            Instantiate(_explosionParticle, this.transform.position, Quaternion.identity);
            base.DestroyProjectile();
        }
    }
}
