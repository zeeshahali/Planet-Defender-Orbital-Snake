using UnityEngine;

public class PlanetHealth : MonoBehaviour
{
    [SerializeField] private EarthConfig config;
    
    private float _heatlh;

    private void Awake()
    {
        _heatlh = config.StartingHealth;
        EventManager.TriggerPlanetHealthUpdate(_heatlh);
    }
    
    private void OnEnable()
    {
        EventManager.OnCollisionWithPlanet += OnCollisionWithPlanet;
    }

    private void OnDisable()
    {
        EventManager.OnCollisionWithPlanet -= OnCollisionWithPlanet;
    }

    private void OnCollisionWithPlanet()
    {
        if (_heatlh <= 0) return;
            
        _heatlh -=  config.DamagePerHit;
        _heatlh = Mathf.Clamp(_heatlh, 0, config.StartingHealth);
        EventManager.TriggerPlanetHealthUpdate(_heatlh);
        
        if(_heatlh <= 0)
            EventManager.TriggerGameOver();
    }
}