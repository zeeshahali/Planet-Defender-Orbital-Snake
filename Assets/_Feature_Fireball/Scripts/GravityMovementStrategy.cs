using UnityEngine;

public class GravityMovementStrategy : BaseMovementStrategy
{
    public GravityMovementStrategy(float speed) : base(speed) { }
    
    public override void Move(Rigidbody rb, Transform currentPosition, Transform targetPosition)
    {
        if (targetPosition == null) return;
        
        Vector3 direction = (targetPosition.position - currentPosition.position).normalized;
        rb.AddForce(direction * _Speed, ForceMode.Acceleration);
    }
}