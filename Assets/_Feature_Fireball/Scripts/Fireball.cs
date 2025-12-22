using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Transform Target;
    public EarthConfig EarthConfig;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    public void Initialize(Transform target)
    {
        Target = target;
    }

    private void FixedUpdate()
    {
        if(Target == null) return;
        
        var direction =  (Target.position - transform.position).normalized;
        _rb.AddForce(direction * EarthConfig.GravityIntensity,  ForceMode.Acceleration);
    }
}