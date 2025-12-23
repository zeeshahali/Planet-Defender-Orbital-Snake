using UnityEngine;

public class ProjectileFactory
{
    private Fireball _fireballPrefab;
    
    public ProjectileFactory(Fireball fireballPrefab)
    {
        _fireballPrefab = fireballPrefab;
    }

    public Fireball CreateFireball(Vector3 position, Transform target, IMovementStrategy movement,
        ILoopDetection loopDetection)
    {
        Fireball fireball = Object.Instantiate(_fireballPrefab, position, Quaternion.identity);
        fireball.SetTarget(target);
        fireball.SetMovementStrategy(movement);
        fireball.SetLoopDetection(loopDetection);
        
        return fireball;
    }
}