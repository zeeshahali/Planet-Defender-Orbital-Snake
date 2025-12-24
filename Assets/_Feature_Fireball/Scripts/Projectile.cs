using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    public abstract class Projectile : MonoBehaviour, ICollisionHandler, IDestructible
    {
        [SerializeField] private MeshRenderer MeshRenderer;

        [SerializeField] private ProjectileReferenceHolder projectileReferenceHolder;

        [SerializeField] protected ProjectileType ProjectileType;

        protected Transform target;
        protected Rigidbody rb;

        protected IMovementStrategy movementStrategy;
        
        protected ILoopDetection _loopDetection;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
        }

        public virtual void UpdateMeshRendererState(bool state)
        {
            MeshRenderer.enabled = state;
        }

        public virtual void UpdateRbConstraints(RigidbodyConstraints constraints)
        {
            rb.constraints = constraints;
        }
        
        public void SetLoopDetection(ILoopDetection loopDetection)
        {
            _loopDetection = loopDetection;
        }

        public virtual void SetTarget(Transform newTarget)
        {
            this.target = newTarget;
        }

        public virtual void SetMovementStrategy(IMovementStrategy newStrategy)
        {
            movementStrategy = newStrategy;
        }

        protected virtual void FixedUpdate()
        {
            if (movementStrategy != null)
                movementStrategy.Move(rb, transform, target);
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }

        public virtual void HandleCollision(Collision collision)
        {
            if (collision.gameObject.CompareTag("Planet"))
            {
                DestroyProjectile();
                EventManager.TriggerCollisionWithPlanet();
            }
        }

        public virtual void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        protected void OnDestroy()
        {
            if (projectileReferenceHolder != null)
                projectileReferenceHolder.Remove(this);
        }
    }
}