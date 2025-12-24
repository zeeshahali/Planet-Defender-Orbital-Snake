using UnityEngine;

public abstract class BaseMovementStrategy : IMovementStrategy
{
    protected readonly float _Speed;
    
    protected BaseMovementStrategy(float speed)
    {
        _Speed = speed;
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