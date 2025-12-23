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
            Destroy(this.gameObject);
            EventManager.TriggerCollisionWithPlanet();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (_loopDetection.IsPointInLoop(transform.position))
            {
                EventManager.TriggerFireballDestroyed();
                Destroy(this.gameObject);
            }
        }
    }

    protected override void OnDestroy()
    {
        Instantiate(_explosionParticle, this.transform.position, Quaternion.identity);
        base.OnDestroy();
    }
}