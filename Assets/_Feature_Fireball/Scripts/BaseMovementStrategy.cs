using UnityEngine;

public abstract class BaseMovementStrategy : IMovementStrategy
{
    protected readonly EarthConfig config;
    
    protected BaseMovementStrategy(EarthConfig config)
    {
        this.config = config;
    }
    
    public abstract void Move(Rigidbody rb, Transform currentPosition, Transform targetPosition);
}

public interface IMovementStrategy
{
    void Move(Rigidbody rb, Transform currentPosition, Transform targetPosition);
}

public interface ICollisionHandler
{
    void HandleCollision(Collision collision);
}

public interface IDestructible
{
    void DestroyProjectile();
}