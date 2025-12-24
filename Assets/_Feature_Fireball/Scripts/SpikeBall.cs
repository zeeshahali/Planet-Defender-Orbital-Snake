using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    public class SpikeBall : Projectile
    {
        private IBodyManipulation SnakeBodyManipulation;

        private bool _canDamageBody = true;
        
        protected override void Awake()
        {
            base.Awake();
            ProjectileType = ProjectileType.SpikeBall;
            _canDamageBody = true;
        }

        public void UpdateBodyManipulation(IBodyManipulation snakeBodyManipulation)
        {
            SnakeBodyManipulation = snakeBodyManipulation;
        }

        public override void HandleCollision(Collision collision)
        {
            base.HandleCollision(collision);

            if (collision.gameObject.CompareTag("Player"))
            {
                DamageBody();
                if (_loopDetection.IsPointInLoop(transform.position))
                {
                    EventManager.TriggerFireballDestroyed();
                    DestroyProjectile();
                }
            }
        }

        private void DamageBody()
        {
            if (!_canDamageBody) return;
            _canDamageBody = false;
            
            SnakeBodyManipulation.ShrinkSnake();
        }
    }
}