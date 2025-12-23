using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private EarthConfig config;
    
    protected Transform target;
    protected Rigidbody rb;

    protected IMovementStrategy movementStrategy;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
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
        if(movementStrategy!=null)
            movementStrategy.Move(rb, transform, target);
    }

    protected virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
}