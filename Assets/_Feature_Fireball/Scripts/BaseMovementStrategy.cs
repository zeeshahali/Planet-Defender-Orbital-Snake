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
    void HandleCollision(Collision collision, GameObject self, ILoopDetection loopDetection);
}

public interface IDestructible
{
    void Destroy();
}