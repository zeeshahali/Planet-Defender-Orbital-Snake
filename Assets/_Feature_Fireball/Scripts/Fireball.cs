using UnityEngine;

public class Fireball : Projectile
{
    private ILoopDetection _loopDetection;
    [SerializeField] private GameObject _explosionParticle;

    public void SetLoopDetection(ILoopDetection loopDetection)
    {
        _loopDetection = loopDetection;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            DestroyFireball();
            EventManager.TriggerCollisionWithPlanet();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (_loopDetection.IsPointInLoop(transform.position))
            {
                EventManager.TriggerFireballDestroyed();
                DestroyFireball();
            }
        }
    }

    private void DestroyFireball()
    {
        Instantiate(_explosionParticle, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}