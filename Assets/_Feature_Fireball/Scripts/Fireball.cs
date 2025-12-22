using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Transform Target;
    public EarthConfig EarthConfig;

    private Rigidbody _rb;
    
    private ILoopDetection _loopDetection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    public void Initialize(Transform target, ILoopDetection loopDetection)
    {
        Target = target;
        _loopDetection = loopDetection;
    }

    private void FixedUpdate()
    {
        if(Target == null) return;
        
        var direction =  (Target.position - transform.position).normalized;
        _rb.AddForce(direction * EarthConfig.GravityIntensity,  ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            Destroy(this.gameObject);
            EventManager.TriggerCameraShake();
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
}