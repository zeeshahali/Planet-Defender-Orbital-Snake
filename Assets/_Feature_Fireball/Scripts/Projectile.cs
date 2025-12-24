using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private MeshRenderer MeshRenderer;
    
    [SerializeField] private ProjectileReferenceHolder projectileReferenceHolder;
    
    protected Transform target;
    protected Rigidbody rb;

    protected IMovementStrategy movementStrategy;
    
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

    protected void OnDestroy()
    {
        if(projectileReferenceHolder!=null)
            projectileReferenceHolder.Remove(this);
    }
}